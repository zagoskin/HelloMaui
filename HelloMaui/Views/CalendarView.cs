using HelloMaui.Views.Abstractions;

namespace HelloMaui.Views;
public class CalendarView : View, ICalendarView
{
    public static readonly BindableProperty FirstDayOfWeekProperty = BindableProperty.Create(
        nameof(FirstDayOfWeek), 
        returnType: typeof(DayOfWeek),
        declaringType: typeof(CalendarView),
        defaultValue: default(DayOfWeek));

    public static readonly BindableProperty MinDateProperty = BindableProperty.Create(
        nameof(MinDate), 
        returnType: typeof(DateTimeOffset),
        declaringType: typeof(CalendarView),
        defaultValue: DateTimeOffset.MinValue);

    public static readonly BindableProperty MaxDateProperty = BindableProperty.Create(
        nameof(MaxDate), 
        returnType: typeof(DateTimeOffset),
        declaringType: typeof(CalendarView),
        defaultValue: DateTimeOffset.MaxValue);

    public static readonly BindableProperty SelectedDateProperty = BindableProperty.Create(
        nameof(SelectedDate),
        returnType: typeof(DateTimeOffset?),
        declaringType: typeof(CalendarView));

    public event EventHandler<SelectedDateChangedEventArgs>? SelectedDateChanged;

    public DayOfWeek FirstDayOfWeek 
    { 
        get => (DayOfWeek)GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    public DateTimeOffset MinDate
    {
        get => (DateTimeOffset)GetValue(MinDateProperty);
        set => SetValue(MinDateProperty, value);
    }

    public DateTimeOffset MaxDate
    {
        get => (DateTimeOffset)GetValue(MaxDateProperty);
        set => SetValue(MaxDateProperty, value);
    }

    public DateTimeOffset? SelectedDate
    {
        get => (DateTimeOffset?)GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    void ICalendarView.OnSelectedDateChanged(DateTimeOffset? selectedDate)
    {
        SelectedDateChanged?.Invoke(this, new SelectedDateChangedEventArgs(selectedDate));
    }
}

public class SelectedDateChangedEventArgs(DateTimeOffset? selectedDate) : EventArgs
{
    public DateTimeOffset? SelectedDate { get; } = selectedDate;
}