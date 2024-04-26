
using CommunityToolkit.Maui.Markup;
using HelloMaui.ViewModels;
using System.Runtime.CompilerServices;

namespace HelloMaui.Pages;
public class DetailsPage : BaseContentPage<DetailsViewModel>
{
    private readonly WeakEventManager _disappearingEventManager = new();
    public DetailsPage(DetailsViewModel viewModel) : base(viewModel)
    {
        this.Bind(TitleProperty, getter: static (DetailsViewModel vm) => vm.LibraryTitle);
        //        Shell.SetBackButtonBehavior(this, new BackButtonBehavior()
        //        {
        //#if ANDROID
        //            TextOverride = "List",
        //#else
        //            TextOverride = "List"   
        //#endif
        //        });

        Content = new VerticalStackLayout()
        {
            Spacing = 12,
            Children =
            {
                new Image()
                    .Center()
                    .Aspect(Aspect.AspectFit)
                    .Size(250)
                    .CenterHorizontal()
                    .Bind(Image.SourceProperty, getter: static (DetailsViewModel vm) => vm.LibraryImageSource),

                new Label()
                    .TextCenter()
                    .Center()
                    .Font(size: 24, bold: true)
                    .Bind(Label.TextProperty, getter: static (DetailsViewModel vm) => vm.LibraryTitle),

                new Label()
                    .TextCenter()
                    .Center()
                    .Font(size: 16, italic: true)
                    .Bind(Label.TextProperty, getter: static (DetailsViewModel vm) => vm.LibraryDescription),

                new Button()
                    .Text("Go Back")
                    .Bind(Button.CommandProperty, getter: static (DetailsViewModel vm) => vm.HandleGoBackCommand)
            }
        }
        .Center()
        .Padding(12);
    }

    // Memory leak example, avoid with weak events
    public event EventHandler? DetailsPageDisappearing
    {
        add => _disappearingEventManager.AddEventHandler(value);
        remove => _disappearingEventManager.RemoveEventHandler(value);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Since we are using weak events, other pages don't need to unsubscribe
        OnDetailsPageDisappearring();
    }

    private void OnDetailsPageDisappearring() => _disappearingEventManager
        .HandleEvent(this, EventArgs.Empty, nameof(DetailsPageDisappearing));
}
