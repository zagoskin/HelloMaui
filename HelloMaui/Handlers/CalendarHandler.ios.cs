using Foundation;
using HelloMaui.Views.Abstractions;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using ObjCRuntime;
using UIKit;

namespace HelloMaui.Handlers;

public partial class CalendarHandler : ViewHandler<ICalendarView, UICalendarView>, IDisposable
{
    UICalendarSelection? _calendarSelection;        

    ~CalendarHandler()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected override UICalendarView CreatePlatformView()
    {
        return new UICalendarView();
    }

    protected override void ConnectHandler(UICalendarView platformView)
    {
        base.ConnectHandler(platformView);

        _calendarSelection = new UICalendarSelectionSingleDate(new CalendarSelectionSingleDateDelegate(VirtualView));
    }

    protected override void DisconnectHandler(UICalendarView platformView)
    {
        base.DisconnectHandler(platformView);

        _calendarSelection?.Dispose();
        _calendarSelection = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (disposing)
        {
            _calendarSelection?.Dispose();
            _calendarSelection = null;
        }
    }

    void ReleaseUnmanagedResources()
    {
        // TODO release unmanaged resources here
    }


    private static void MapFirstDayOfWeek(CalendarHandler handler, ICalendarView calendarView)
    {
        handler.PlatformView.Calendar.FirstWeekDay = (nuint)calendarView.FirstDayOfWeek;
    }

    private static void MapMinDate(CalendarHandler handler, ICalendarView calendarView)
    {
        SetDateRange(handler, calendarView);
    }

    private static void MapMaxDate(CalendarHandler handler, ICalendarView calendarView)
    {
        SetDateRange(handler, calendarView);
    }

    private static void MapSelectedDate(CalendarHandler handler, ICalendarView calendarView)
    {
        if (handler._calendarSelection is UICalendarSelectionSingleDate selection)
        {
            MapSingleDateSelection(selection, calendarView);
        }
    }

    private static void MapSingleDateSelection(UICalendarSelectionSingleDate selection, ICalendarView calendarView)
    {
        if (calendarView.SelectedDate is null)
        {
            selection.SetSelectedDate(null, true);
            return;
        }

        selection.SetSelectedDate(new NSDateComponents
        {
            Year = calendarView.SelectedDate.Value.Year,
            Month = calendarView.SelectedDate.Value.Month,
            Day = calendarView.SelectedDate.Value.Day
        }, true);
    }

    private static void SetDateRange(CalendarHandler calendar, ICalendarView virtualView)
    {
        var fromDateComponents = virtualView.MinDate.Date.ToNSDate();
        var toDateComponents = virtualView.MaxDate.Date.ToNSDate();

        var calendarViewDateRange = new NSDateInterval(fromDateComponents, toDateComponents);
        calendar.PlatformView.AvailableDateRange = calendarViewDateRange;
    }

    private sealed class CalendarSelectionSingleDateDelegate(ICalendarView calendarView) : IUICalendarSelectionSingleDateDelegate
    {
        public NativeHandle Handle { get; }

        public void DidSelectDate(UICalendarSelectionSingleDate selection, NSDateComponents? dateComponents)
        {
            selection.SelectedDate = dateComponents;
            calendarView.SelectedDate = dateComponents?.Date.ToDateTime();
            calendarView.OnSelectedDateChanged(dateComponents?.Date.ToDateTime());
                
        }

        public void Dispose()
        {
            
        }
    }    
}
