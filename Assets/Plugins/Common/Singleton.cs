
public class Singleton<T> where T : class, new()
{
	static volatile T _instance;
	static object _lock = new object ();

	static Singleton ()
	{
	}

	public static T Instance {
		get {
			if (_instance == null)
				lock (_lock) {
					if (_instance == null) {
						_instance = new T();
					}
				}

			return _instance;
		}
	}
}
