// Created by Laxale 14.11.2016
//
//


namespace Freengy.CommonResources.Controls 
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
            get { return (ControlTemplate)this.GetValue(FlipPanel.SlideButtonTemplateProperty); }
            set { this.SetValue(FlipPanel.SlideButtonTemplateProperty, value); }
        }

        public bool IsFrontPanelActive 
        {
            get { return (bool)this.GetValue(FlipPanel.IsFrontPanelActiveProperty); }

            set
            {
                this.SetValue(FlipPanel.IsFrontPanelActiveProperty, value);

                this.FlipPanels();
            }
        }

        public UserControl FrontPanel 
        {
            get { return (UserControl)this.GetValue(FlipPanel.FrontPanelProperty); }
            set { this.SetValue(FlipPanel.FrontPanelProperty, value); }
        }

        public UserControl BackPanel 
        {
            get { return (UserControl)this.GetValue(FlipPanel.BackPanelProperty); }
            set { this.SetValue(FlipPanel.BackPanelProperty, value); }
        }

        public static readonly DependencyProperty IsFrontPanelActiveProperty =
            DependencyProperty.RegisterAttached(nameof(FlipPanel.IsFrontPanelActive), typeof(bool), typeof(FlipPanel),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty FrontPanelProperty =
            DependencyProperty.Register(nameof(FlipPanel.FrontPanel), typeof(UserControl), typeof(FlipPanel));

        public static readonly DependencyProperty BackPanelProperty =
            DependencyProperty.Register(nameof(FlipPanel.BackPanel), typeof(UserControl), typeof(FlipPanel));

        public static readonly DependencyProperty SlideButtonTemplateProperty =
            DependencyProperty.Register(nameof(FlipPanel.SlideButtonTemplate), typeof(ControlTemplate), typeof(FlipPanel));

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

            Storyboard.SetTargetProperty(fadeInBackStoryBoard,   new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTargetProperty(fadeOutBackStoryBoard,  new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTargetProperty(fadeInFrontStoryBoard,  new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTargetProperty(fadeOutFrontStoryBoard, new PropertyPath(UIElement.OpacityProperty));
            
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