using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Animation;

namespace SlidingFragments
{
    [Activity(Label = "SlidingFragments", MainLauncher = true, Icon = "@drawable/icon")]
    public class SlidingFragments : Activity, FragmentManager.IOnBackStackChangedListener
    {
        ImageFragment mImageFragment;
        TextFragment mTextFragment;
        View mDarkHoverView;

        bool mDidSlideOut = false;
        bool mIsAnimating = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.sliding_fragments_layout);

            mDarkHoverView = FindViewById(Resource.Id.dark_hover_view);
            mDarkHoverView.Alpha = 0;

            mImageFragment = FragmentManager.FindFragmentById<ImageFragment>(Resource.Id.move_fragment);
            mTextFragment = new TextFragment();

            FragmentManager.AddOnBackStackChangedListener(this);

            mImageFragment.Click += OnClick;
            mTextFragment.Click += OnClick;
            mTextFragment.AnimationEnd += OnAnimationEnd;
            mDarkHoverView.Click += OnClick;
        }

        void OnAnimationEnd(object sender, EventArgs e)
        {
            mIsAnimating = false;
        }

        private void OnClick(object sender, EventArgs e)
        {
            SwitchFragments();
        }

        /**
         * This method is used to toggle between the two fragment states by
         * calling the appropriate animations between them. The entry and exit
         * animations of the text fragment are specified in R.animator resource
         * files. The entry and exit animations of the image fragment are
         * specified in the slideBack and slideForward methods below. The reason
         * for separating the animation logic in this way is because the translucent
         * dark hover view must fade in at the same time as the image fragment
         * animates into the background, which would be difficult to time
         * properly given that the setCustomAnimations method can only modify the
         * two fragments in the transaction.
         */
        private void SwitchFragments()
        {
            if (mIsAnimating)
            {
                return;
            }
            mIsAnimating = true;
            if (mDidSlideOut)
            {
                mDidSlideOut = false;
                FragmentManager.PopBackStack();
            }
            else
            {
                mDidSlideOut = true;
                SlideBack();
            }
        }

        public void OnBackStackChanged()
        {
            if (!mDidSlideOut)
            {
                SlideForward();
            }
        }

        /**
         * This method animates the image fragment into the background by both
         * scaling and rotating the fragment's view, as well as adding a
         * translucent dark hover view to inform the user that it is inactive.
         */
        public void SlideBack()
        {
            View movingFragmentView = mImageFragment.View;

            PropertyValuesHolder rotateX = PropertyValuesHolder.OfFloat("rotationX", 40f);
            PropertyValuesHolder scaleX = PropertyValuesHolder.OfFloat("scaleX", 0.8f);
            PropertyValuesHolder scaleY = PropertyValuesHolder.OfFloat("scaleY", 0.8f);
            ObjectAnimator movingFragmentAnimator = ObjectAnimator.
                    OfPropertyValuesHolder(movingFragmentView, rotateX, scaleX, scaleY);

            ObjectAnimator darkHoverViewAnimator = ObjectAnimator.
                    OfFloat(mDarkHoverView, "alpha", 0.0f, 0.5f);

            ObjectAnimator movingFragmentRotator = ObjectAnimator.
                    OfFloat(movingFragmentView, "rotationX", 0);
            movingFragmentRotator.StartDelay = Resources.GetInteger(Resource.Integer.half_slide_up_down_duration);

            AnimatorSet s = new AnimatorSet();
            s.PlayTogether(movingFragmentAnimator, darkHoverViewAnimator, movingFragmentRotator);
            s.AnimationEnd += delegate
            {
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                transaction.SetCustomAnimations(Resource.Animator.slide_fragment_in, 0, 0, Resource.Animator.slide_fragment_out);
                transaction.Add(Resource.Id.move_to_back_container, mTextFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };
            s.Start();
        }

        /**
         * This method animates the image fragment into the foreground by both
         * scaling and rotating the fragment's view, while also removing the
         * previously added translucent dark hover view. Upon the completion of
         * this animation, the image fragment regains focus since this method is
         * called from the onBackStackChanged method.
         */
        public void SlideForward()
        {
            View movingFragmentView = mImageFragment.View;

            PropertyValuesHolder rotateX = PropertyValuesHolder.OfFloat("rotationX", 40f);
            PropertyValuesHolder scaleX = PropertyValuesHolder.OfFloat("scaleX", 1.0f);
            PropertyValuesHolder scaleY = PropertyValuesHolder.OfFloat("scaleY", 1.0f);
            ObjectAnimator movingFragmentAnimator = ObjectAnimator.
                    OfPropertyValuesHolder(movingFragmentView, rotateX, scaleX, scaleY);

            ObjectAnimator darkHoverViewAnimator = ObjectAnimator.
                    OfFloat(mDarkHoverView, "alpha", 0.5f, 0.0f);

            ObjectAnimator movingFragmentRotator = ObjectAnimator.
                    OfFloat(movingFragmentView, "rotationX", 0);
            movingFragmentRotator.StartDelay = Resources.GetInteger(Resource.Integer.half_slide_up_down_duration);

            AnimatorSet s = new AnimatorSet();
            s.PlayTogether(movingFragmentAnimator, movingFragmentRotator, darkHoverViewAnimator);
            s.StartDelay = Resources.GetInteger(Resource.Integer.slide_up_down_duration);
            s.AnimationEnd += delegate
            {
                mIsAnimating = false;
            };
            s.Start();
        }
    }
}