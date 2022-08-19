package Game
{
	import flash.display.Loader;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.net.URLRequest;
	import flash.net.Socket;
	import flash.external.ExternalInterface;
	import flash.system.Security;
	import flash.events.MouseEvent;
	
	/**
	 * ...
	 * @author Pat Tran
	 */
	public class Main extends Sprite 
	{
		private var loader:Loader;
		private var movie : String;
		public function Main() 
		{
			// allow to load all swf file
			Security.allowDomain("*");
			
			if (stage) {
				loader = new Loader();
				// test sending message
				ExternalInterface.call("as3ToC#", "Hello world");
				// retrieving movie variable from C# 
				initMovie();
				// add zoom in/out support
				zoom();
			}
		}
		
		private function initMovie():void {
			ExternalInterface.addCallback("loadMovie", function (text : String) : String
			{
				this.movie = text;
				loader.load(new URLRequest(this.movie));
				stage.addChild(loader);
				return this.movie; // optional, just to test the data
			});
		}
		
		private function zoom():void {
			ExternalInterface.addCallback("zoomIn", function (text : String) : String
			{
				this.movie = text;
				loader.load(new URLRequest(this.movie));
				stage.addChild(loader);
				return this.movie; // optional, just to test the data
			});
		}
		
	}
	
}