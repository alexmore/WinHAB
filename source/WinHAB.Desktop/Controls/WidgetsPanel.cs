using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Widgets;
using WinHAB.Desktop.Configuration;

namespace WinHAB.Desktop.Controls
{
  public class WidgetsPanel : Panel
  {
    public WidgetsPanel() : base()
    {
      MinHeight = AppConstants.WidgetLargeSize.Height + AppConstants.WidgetMarging;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      var rowsCount = GetRowsCount(availableSize);
      rowsCount = rowsCount >= 0 ? rowsCount : 1;

      var map = GetWidgetsMap(rowsCount);
      var columnsCount = GetColumnsCount(map);

      return new Size(columnsCount * (AppConstants.WidgetMediumFullWidth), availableSize.Height);
    }
    
    protected override Size ArrangeOverride(Size finalSize)
    {
      var rowsCount = GetRowsCount(finalSize);
      if (rowsCount == 0) rowsCount = 1;

      var map = GetWidgetsMap(rowsCount);
      var columnsCount = GetColumnsCount(map);

      for (int column = 0; column < columnsCount; column++)
        for (int row = 0; row < rowsCount; row++)
        {
          if (map[row][column] != null && map[row][column].Content != null)
          {
            var mapItem = map[row][column].Content;
            var rect = new Rect(new Point(column * AppConstants.WidgetMediumFullWidth, row * AppConstants.WidgetMediumFullHeight), AppConstants.GetWidgetSize(map[row][column].Size));
            mapItem.RenderSize =AppConstants.GetWidgetSize(map[row][column].Size);
            mapItem.Arrange(rect);
          }
        }
      return base.ArrangeOverride(finalSize);
    }

    class MapItem
    {
      public MapItem(ContentPresenter content, WidgetSize size) { Content = content; Size = size; }

      public ContentPresenter Content { get; set; }
      public WidgetSize Size { get; set; }
    }
   
    private int GetColumnsCount(List<MapItem>[] map)
    {
      if (map[0].Count == 0) return 0;
      
      var columnsCount = map[0].Count-1;

      var hasWidgetInLastColumn = false;
      if (map.Any(t => t[map[0].Count - 1] != null))
        columnsCount++;

      return columnsCount;
    }

    private int GetRowsCount(Size areaSize)
    {
      return (int)Math.Truncate(areaSize.Height / AppConstants.WidgetMediumFullHeight);
    }

    void AddMapColumn(List<MapItem>[] map)
    {
      foreach (var j in map) { j.Add(null); j.Add(null); }
    }

    private bool CanSetWidget(List<MapItem>[] map, int row, int column, WidgetSize size)
    {
      return (size == WidgetSize.Meduim && map[row][column] == null) ||
             (size == WidgetSize.Wide && column%2 == 0 && map[row][column] == null && map[row][column + 1] == null) ||
             (size == WidgetSize.Large && column%2 == 0 && row < map.Length - 2 && map[row][column] == null &&
                map[row][column + 1] == null && map[row + 1][column] == null && map[row + 1][column + 1] == null);
    }

    private void SetWidget(ContentPresenter content, List<MapItem>[] map, int row, int column, WidgetSize size)
    {
      if (size == WidgetSize.Meduim) map[row][column] = new MapItem(content, WidgetSize.Meduim);
      
      if (size == WidgetSize.Wide)
      {
        map[row][column] = new MapItem(content, WidgetSize.Wide);
        map[row][column + 1] = new MapItem(null, WidgetSize.Wide);
      }
      
      if (size == WidgetSize.Large)
      {
        map[row][column] = new MapItem(content, WidgetSize.Large);
        map[row][column + 1] = new MapItem(null, WidgetSize.Large);
        map[row + 1][column] = new MapItem(null, WidgetSize.Large);
        map[row + 1][column + 1] = new MapItem(null, WidgetSize.Large);
      }
    }

    bool? FindAndPlace(List<MapItem>[] map, ContentPresenter content)
    {
      var widget = content.Content as WidgetModelBase;
      if (widget == null) return null;

      var columnsCount = map[0].Count;
      var column = 0;

      while (column < columnsCount)
      {
        for (int row = 0; row < map.Length; row++)
        {
          if (CanSetWidget(map, row, column, widget.Size))
          {
            SetWidget(content, map, row, column, widget.Size);
            return true;
          }
          
          if (widget.Size == WidgetSize.Meduim && CanSetWidget(map, row, column + 1, widget.Size))
          {
            SetWidget(content, map, row, column + 1, widget.Size);
            return true;
          }
        }

        column += 2;
      }

      return false;
    }

    List<MapItem>[] GetWidgetsMap(int rowsCount)
    {
      var map = new List<MapItem>[rowsCount];
      for (var i = 0; i < rowsCount; i++) map[i] = new List<MapItem>();

      foreach (var child in Children)
      {
        var isWidgetAdded = FindAndPlace(map, child as ContentPresenter);
        if (isWidgetAdded.HasValue && !isWidgetAdded.Value)
        {
          AddMapColumn(map);
          FindAndPlace(map, child as ContentPresenter); // Try again
        }
      }

      return map;
    }
  }
}