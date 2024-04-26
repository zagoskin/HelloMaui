using HelloMaui.Views.Abstractions;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;
namespace HelloMaui.Handlers;
public partial class CalendarHandler : ViewHandler<ICalendarView, CalendarView>
{
    protected override CalendarView CreatePlatformView()
    {
        return new CalendarView();
    }
    protected override void ConnectHandler(CalendarView nativeView)
    {
        base.ConnectHandler(nativeView);
        nativeView.SelectedDatesChanged += HandleSelectedDatesChanged;
    }
    protected override void DisconnectHandler(CalendarView nativeView)
    {
        nativeView.SelectedDatesChanged -= HandleSelectedDatesChanged;
        base.DisconnectHandler(nativeView);
    }

    private static void MapFirstDayOfWeek(CalendarHandler handler, ICalendarView calendarView)
    {
        handler.PlatformView.FirstDayOfWeek = (Windows.Globalization.DayOfWeek)calendarView.FirstDayOfWeek;
    }

    private static void MapMinDate(CalendarHandler handler, ICalendarView calendarView)
    {
        handler.PlatformView.MinDate = calendarView.MinDate;
    }

    private static void MapMaxDate(CalendarHandler handler, ICalendarView calendarView)
    {
        handler.PlatformView.MaxDate = calendarView.MaxDate;
    }

    private static void MapSelectedDate(CalendarHandler handler, ICalendarView calendarView)
    {
        handler.PlatformView.SelectedDates.Clear();
        if (calendarView.SelectedDate is null)
        {
            return;
        }

        handler.PlatformView.SelectedDates.Add(calendarView.SelectedDate.Value);
        handler.PlatformView.SetDisplayDate(calendarView.SelectedDate.Value);
    }

    private void HandleSelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
    {
        PlatformView.SelectedDatesChanged -= HandleSelectedDatesChanged;

        if (args.AddedDates.Count is 0)
        {
            VirtualView.SelectedDate = null;
        }

        else if (args.AddedDates.Count > 0)
        {
            VirtualView.SelectedDate = args.AddedDates[0];
        }
        
        VirtualView.OnSelectedDateChanged(VirtualView.SelectedDate);
        PlatformView.SelectedDatesChanged += HandleSelectedDatesChanged;
    }
}
