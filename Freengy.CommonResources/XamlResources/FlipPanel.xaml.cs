// Created by Laxale 14.11.2016
//
//


namespace Freengy.CommonResources.XamlResources 
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;


    public static class AnimationNames 
    {
        internal const string FadeInBackStoryBoardName  = "FadeInBackStoryBoard";
        internal const string FadeInFrontStoryBoardName = "FadeInFrontStoryBoard";

        internal const string FadeOutBackStoryBoardName  = "FadeOutBackStoryBoard";
        internal const string FadeOutFrontStoryBoardName = "FadeOutFrontStoryBoard";
    }

    /// <summary>
    /// Control represents two-side panel with side-attached panels
    /// </summary>
    public partial class FlipPanel : UserControl 
    {
        #region vars

        private bool showFrontInvoked = true;

        private readonly BeginStoryboard beginShowBackStoryboard = new BeginStoryboard
        {
            Storyboard = new Storyboard { Children = new TimelineCollection() }
        };
        private readonly BeginStoryboard beginShowFrontStoryboard = new BeginStoryboard
        {
            Storyboard = new Storyboard { Children = new TimelineCollection() }
        };

        #endregion vars


        public FlipPanel() 
        {
            this.InitializeComponent();

            this.FillAnimations();
        }


        #region properties
        
        public ControlTemplate SlideButtonTemplate 
        {
            get { return (ControlTemplate)GetValue(FlipPanel.SlideButtonTemplateProperty); }
            set { SetValue(FlipPanel.SlideButtonTemplateProperty, value); }
        }

        public bool IsFrontPanelActive 
        {
            get { return (bool)GetValue(IsFrontPanelActiveProperty); }

            set
            {
                SetValue(IsFrontPanelActiveProperty, value);

                this.FlipPanels();
            }
        }

        public UserControl FrontPanel 
        {
            get { return (UserControl)GetValue(FlipPanel.FrontPanelProperty); }
            set { SetValue(FlipPanel.FrontPanelProperty, value); }
        }

        public UserControl BackPanel 
        {
            get { return (UserControl)GetValue(FlipPanel.BackPanelProperty); }
            set { SetValue(FlipPanel.BackPanelProperty, value); }
        }

        public static readonly DependencyProperty IsFrontPanelActiveProperty =
            DependencyProperty.RegisterAttached(nameof(IsFrontPanelActive), typeof(bool), typeof(FlipPanel),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty FrontPanelProperty =
            DependencyProperty.Register(nameof(FrontPanel), typeof(UserControl), typeof(FlipPanel));

        public static readonly DependencyProperty BackPanelProperty =
            DependencyProperty.Register(nameof(BackPanel), typeof(UserControl), typeof(FlipPanel));

        public static readonly DependencyProperty SlideButtonTemplateProperty =
            DependencyProperty.Register(nameof(SlideButtonTemplate), typeof(ControlTemplate), typeof(FlipPanel));

        #endregion properties


        private void FlipButton_Click(object sender, RoutedEventArgs e) 
        {
            this.FlipPanels();
        }

        private void FlipPanels() 
        {
            if (this.showFrontInvoked)
            {
                this.beginShowBackStoryboard.Storyboard.Begin();
                
                this.showFrontInvoked = false;
            }
            else
            {
                this.beginShowFrontStoryboard.Storyboard.Begin();

                this.showFrontInvoked = true;
            }
        }

        private void FillAnimations() 
        {
            ParallelTimeline showBackTimeLine  = (ParallelTimeline)this.Resources["ShowBackTimeLine"];
            ParallelTimeline showFrontTimeLine = (ParallelTimeline)this.Resources["ShowFrontTimeLine"];

            showBackTimeLine.Completed  += this.ShowBackTimeLineCompleted;
            showFrontTimeLine.Completed += this.ShowFrontTimeLineCompleted; ;
            
            Storyboard fadeInFrontStoryBoard =
                (Storyboard)showFrontTimeLine
                    .Children.First(storyBoard => storyBoard.Name == AnimationNames.FadeInFrontStoryBoardName);

            Storyboard fadeInBackStoryBoard =
                (Storyboard)showBackTimeLine
                    .Children.First(storyBoard => storyBoard.Name == AnimationNames.FadeInBackStoryBoardName);

            Storyboard fadeOutFrontStoryBoard =
                (Storyboard)showBackTimeLine
                    .Children.First(storyBoard => storyBoard.Name == AnimationNames.FadeOutFrontStoryBoardName);

            Storyboard fadeOutBackStoryBoard =
                (Storyboard)showFrontTimeLine
                    .Children.First(storyBoard => storyBoard.Name == AnimationNames.FadeOutBackStoryBoardName);

            Storyboard.SetTarget(fadeInBackStoryBoard,  this.BackPanelHolder);
            Storyboard.SetTarget(fadeOutBackStoryBoard, this.BackPanelHolder);
            Storyboard.SetTarget(fadeInFrontStoryBoard,  this.FrontPanelHolder);
            Storyboard.SetTarget(fadeOutFrontStoryBoard, this.FrontPanelHolder);

            Storyboard.SetTargetProperty(fadeInBackStoryBoard,   new PropertyPath(OpacityProperty));
            Storyboard.SetTargetProperty(fadeOutBackStoryBoard,  new PropertyPath(OpacityProperty));
            Storyboard.SetTargetProperty(fadeInFrontStoryBoard,  new PropertyPath(OpacityProperty));
            Storyboard.SetTargetProperty(fadeOutFrontStoryBoard, new PropertyPath(OpacityProperty));
            
            this.beginShowBackStoryboard.Storyboard.Children.Add(showBackTimeLine);
            this.beginShowFrontStoryboard.Storyboard.Children.Add(showFrontTimeLine);
        }

        private void ShowBackTimeLineCompleted(object sender, EventArgs e) 
        {
            this.BackPanelHolder.Visibility  = Visibility.Visible;
            this.FrontPanelHolder.Visibility = Visibility.Collapsed;
        }

        private void ShowFrontTimeLineCompleted(object sender, EventArgs e) 
        {
            this.FrontPanelHolder.Visibility = Visibility.Visible;
            this.BackPanelHolder.Visibility  = Visibility.Collapsed;
        }
    }
}