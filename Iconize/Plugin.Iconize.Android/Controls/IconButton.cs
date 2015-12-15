using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Java.Lang;

namespace Plugin.Iconize.Android.Controls
{
    public class IconButton : Button, IHasOnViewAttachListener
    {
        private HasOnViewAttachListenerDelegate _delegate;

        public IconButton(Context context)
            : base(context)
        {
            Init();
        }

        public IconButton(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Init();
        }

        public IconButton(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            Init();
        }

        public IconButton(Context context, IAttributeSet attrs, Int32 defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        private void Init()
        {
            TransformationMethod = null;
        }

        public override void SetText(ICharSequence text, BufferType type)
        {
            base.SetText(this.Compute(Context, text), type);
        }

        public void SetOnViewAttachListener(IOnViewAttachListener listener)
        {
            if (_delegate == null)
                _delegate = new HasOnViewAttachListenerDelegate(this);
            _delegate.SetOnViewAttachListener(listener);
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            _delegate.OnAttachedToWindow();
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            _delegate.OnDetachedFromWindow();
        }
    }
}