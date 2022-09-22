package crc6473a08a4a3a129883;


public class SpaceView
	extends crc6499cc3f8d6dc23bc6.Page
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("UnoTest.SpaceView, UnoTest.Mobile", SpaceView.class, __md_methods);
	}


	public SpaceView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == SpaceView.class)
			mono.android.TypeManager.Activate ("UnoTest.SpaceView, UnoTest.Mobile", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
