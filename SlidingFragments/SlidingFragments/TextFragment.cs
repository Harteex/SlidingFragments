using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Animation;

namespace SlidingFragments
{
    public class TextFragment : Fragment
    {
        public event EventHandler Click;
        protected virtual void OnClick(EventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }

        public event EventHandler AnimationEnd;
        protected virtual void OnAnimationEnd(EventArgs e)
        {
            if (AnimationEnd != null)
                AnimationEnd(this, e);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
                                 Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.text_fragment, container, false);

            view.Click += view_Click;

            return view;
        }

        void view_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        public override Animator OnCreateAnimator(FragmentTransit transit, bool enter, int nextAnim)
        {
            int id = enter ? Resource.Animator.slide_fragment_in : Resource.Animator.slide_fragment_out;
            Animator anim = AnimatorInflater.LoadAnimator(Activity, id);
            if (enter)
                anim.AnimationEnd += anim_AnimationEnd;

            return anim;
        }

        void anim_AnimationEnd(object sender, EventArgs e)
        {
            OnAnimationEnd(e);
        }
    }
}