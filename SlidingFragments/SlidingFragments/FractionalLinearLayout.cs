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
using Android.Util;
using Java.Interop;

namespace SlidingFragments
{
    public class FractionalLinearLayout : LinearLayout
    {
        private float mYFraction;
        private int mScreenHeight;

        public FractionalLinearLayout(Context context)
            : base(context)
        {
        }

        public FractionalLinearLayout(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            mScreenHeight = h;
            SetY(mScreenHeight);
        }

        [Export]
        public float getYFraction()
        {
            return mYFraction;
        }

        [Export]
        public void setYFraction(float yFraction)
        {
            mYFraction = yFraction;
            SetY((mScreenHeight > 0) ? (mScreenHeight - yFraction * mScreenHeight) : 0);
        }
    }
}