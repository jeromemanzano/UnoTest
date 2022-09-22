package crc6473a08a4a3a129883;


public class ChatView
	extends crc6499cc3f8d6dc23bc6.Page
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("UnoTest.ChatView, UnoTest.Mobile", ChatView.class, __md_methods);
	}


	public ChatView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ChatView.class)
			mono.android.TypeManager.Activate ("UnoTest.ChatView, UnoTest.Mobile", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
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
