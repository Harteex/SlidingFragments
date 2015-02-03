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

namespace SlidingFragments
{
    public class ImageFragment : Fragment
    {
        public event EventHandler Click;

        protected virtual void OnClick(EventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
                                 Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.image_fragment, container, false);

            view.Click += view_Click;

            return view;
        }

        void view_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }
    }
}