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
	import flash.display.StageScaleMode;
	import flash.display.StageAlign;
	
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
				// ExternalInterface.call("as3ToC#", "Hello world");
				
				// retrieving movie variable from C# 
				initMovie();
				// add zoom in/out support
				zoomMap();
				zoomMenu();
				resetZoomDefault();
				
				// restart the game
				restartGame();
			}
		}
		
		/**
		 * Initialize loading the SWF content
		 */
		private function initMovie():void {
			ExternalInterface.addCallback("loadMovie", function (text : String) : String
			{
				this.movie = text;
				loader.load(new URLRequest(this.movie));
				loader.contentLoaderInfo.addEventListener(Event.COMPLETE, handleLoaded);
				
				return this.movie; // optional, just to test the data
			});
		}
		
		/**
		 * Add the SWF content to the stage when it is loaded
		 * @param	evt
		 */
		private function handleLoaded (evt:Event):void {
			stage.addChild(loader.content);
		}
		
		/**
		 * A function to add the ExternalInterface to accept call from outside (C#)
		 * Restart the game
		 */
		private function restartGame():void {
			
			ExternalInterface.addCallback("restartGame", function(text : String) : String
			{
				stage.removeChild(loader.content);
				loader.unload();
				loader.load(new URLRequest(this.movie));
				loader.contentLoaderInfo.addEventListener(Event.COMPLETE, handleLoaded);
				
				return ""; // for testing purpose
			});
		}
		
		/**
		 * A function to add the ExternalInterface to accept call from outside (C#)
		 * Resizing the map requires a lot of processing to make sure the camera doesnt look too bad
		 */
		private function zoomMap():void {
			
			ExternalInterface.addCallback("zoomMap", function(text : String) : String
			{
				// check if the map is loaded up
				var mapWidth:Number = stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["width"];
				var mapHeight:Number = stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["height"];
				// only change the size of the map if it exists
				if (mapWidth != 0 && mapHeight != 0) {
					var scale:Number = parseFloat(text);
					var value:int = 0;
					stage.scaleMode = StageScaleMode.NO_SCALE;
					// if the scale is changed back to 1
					if (scale == 1.0) {
						stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["scaleX"] = 1;
						stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["scaleY"] = 1;
					} else {
						/**
						 * UI (without map): index = 2
						 * Only map: index = 0
						 */						
						stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["scaleX"] = scale;
						stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["scaleY"] = scale;
						
						if (scale < 1) {
							stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["scaleX"] *= (stage.stageWidth / stage.stageHeight);
							var parentMapWidth:Number = stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["width"];
							var parentMapHeight:Number = stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["height"];
							var greaterNumber:Number = (parentMapWidth > parentMapHeight) ? parentMapWidth : parentMapHeight;
							
							stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["width"] = greaterNumber;
							stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["height"] = greaterNumber;
						}
					}
				}		
				
				return "Width: " + stage.stageWidth + ", height: " + stage.stageHeight +
					";\n Map Width: " + mapWidth + ", map height: " + mapHeight; // for testing purpose
			});
		}
		
		/**
		 * A function to add the ExternalInterface to accept call from outside (C#)
		 * Resizing the menus, leaving only the map intact
		 */
		private function zoomMenu():void {
			
			ExternalInterface.addCallback("zoomMenu", function(text : String) : String
			{
				var scale:Number = parseFloat(text);
				stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](2)["scaleX"] = scale;
				stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](2)["scaleY"] = scale;
				return ""; // for testing purpose
			});
		}
		
		/**
		 * A function to add the ExternalInterface to accept call from outside (C#)
		 * Reset the zoom levels to default
		 */
		private function resetZoomDefault():void {
			
			ExternalInterface.addCallback("resetZoom", function(text : String) : String
			{
				stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](2)["scaleX"] = 1;
				stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](2)["scaleY"] = 1;
				
				stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](0)["scaleX"] = 1;
				stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](0)["scaleY"] = 1;
				return ""; // for testing purpose
			});
		}
		
	}
	
}