using Android.Widget;
using HelloMaui.Views.Abstractions;
using Microsoft.Maui.Handlers;
using System.Runtime.CompilerServices;

namespace HelloMaui.Handlers;
public partial class CalendarHandler : ViewHandler<ICalendarView, CalendarView>
{
    protected override CalendarView CreatePlatformView()
    {
        return new CalendarView(Context);
    } 

    protected override void ConnectHandler(CalendarView platformView)
    {
        base.ConnectHandler(platformView);
        platformView.DateChange += HandleDateChanged;
    }

    protected override void DisconnectHandler(CalendarView platformView)
    {
        base.DisconnectHandler(platformView);
        platformView.DateChange -= HandleDateChanged;
    }

    private static void MapFirstDayOfWeek(CalendarHandler handler, ICalendarView calendarView)
    {
        handler.PlatformView.FirstDayOfWeek = (int)calendarView.FirstDayOfWeek;
    }

    private static void MapMinDate(CalendarHandler handler, ICalendarView calendarView)
    {
        handler.PlatformView.MinDate = calendarView.MinDate.ToUnixTimeMilliseconds();
    }

    private static void MapMaxDate(CalendarHandler handler, ICalendarView calendarView)
    {
        handler.PlatformView.MaxDate = calendarView.MaxDate.ToUnixTimeMilliseconds();
    }

    private static void MapSelectedDate(CalendarHandler handler, ICalendarView calendarView)
    {
        if (calendarView.SelectedDate is null)
        {
            return;
        }

        handler.PlatformView.SetDate(
            calendarView.SelectedDate.Value.ToUnixTimeMilliseconds(),
            animate: true,
            center: true);
    }

    private void HandleDateChanged(object? sender, CalendarView.DateChangeEventArgs e)
    {
        PlatformView.DateChange -= HandleDateChanged;

        VirtualView.SelectedDate = new DateTime(e.Year, e.Month + 1, e.DayOfMonth);
        VirtualView.OnSelectedDateChanged(VirtualView.SelectedDate);

        PlatformView.DateChange += HandleDateChanged;
    }
}