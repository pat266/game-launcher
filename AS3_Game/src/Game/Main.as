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
		
		private var defaultVal : Boolean;
		private var originalWidth : Number;
		private var originalHeight : Number;
		
		public function Main() 
		{
			// allow to load all swf file
			Security.allowDomain("*");
			
			if (stage) {
				loader = new Loader();
				defaultVal = true;
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
				loader.contentLoaderInfo.addEventListener(Event.COMPLETE, handleLoaded );
				
				return this.movie; // optional, just to test the data
			});
		}
		
		private function handleLoaded (evt:Event):void {
			stage.addChild(loader.content);
		}
		
		private function zoom():void {
			
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
						originalWidth = stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["width"];
						originalHeight = stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["height"];
						
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
				
				/**
				// reprocess the map
				var mapWidth:Number = stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["width"];
				var mapHeight:Number = stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["height"];
				var parentMapWidth:Number = stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["width"];
				var parentMapHeight:Number = stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["height"];
				
				
				 
				// choose a greater number
				var greaterNumber:Number = (parentMapWidth > parentMapHeight) ? parentMapWidth : parentMapHeight;
				// check on the right
				if (!checkMapFitRight(mapWidth, parentMapWidth)) {
					// stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["width"] = originalWidth;
					stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["width"] = greaterNumber;
					stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["height"] = greaterNumber;
					mapWidth = greaterNumber;
				}
				
				// check on the bottom
				if (!checkMapFitBottom(mapHeight, parentMapHeight)) {
					// stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["height"] = originalHeight;
					stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["width"] = greaterNumber;
					stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](value)["height"] = greaterNumber;
					mapHeight = greaterNumber;
				}
				*/
				
				
				return "Width: " + stage.stageWidth + ", height: " + stage.stageHeight +
					";\n Map Width: " + mapWidth + ", map height: " + mapHeight; // for testing purpose
			});
		}
		
		/**
		 * Helper Function:
		 * Check if the current map (stage's child) not have empty space on the right
		 * Not fit when Map's parent width > Map's width
		 * Set Map's X value by Map's parent width - Map's width
		 */
		private function checkMapFitRight(mapWidth:Number, parentMapWidth:Number): Boolean {
			return parentMapWidth == mapWidth;
		}
				
		/**
		 * Helper Function:
		 * Check if the current map (stage's child) not have empty space on the bottom
		 * Not fit when Map's parent height > Map's height
		 * Set Map's Y value by Map's parent height - Map's height
		 */
		private function checkMapFitBottom(mapHeight:Number, parentMapHeight:Number): Boolean {
			return parentMapHeight == mapHeight;
		}
		
		/**
		 * Helper Function:
		 * Calculate the difference between 2 values
		 */
		private function calculateDifference(num1:Number, num2:Number): Number {
			return num2 - num1;
		}
		
		
		/**
		 * Helper Function:
		 * Check if the current map (stage's child) not have empty space on the left
		 */
		private function checkMapFitLeft(): Boolean {
			return false;
		}
		
		/**
		 * Helper Function:
		 * Check if the current map (stage's child) not have empty space on the top
		 */
		private function checkMapFitTop(): Boolean {
			return false;
		}
		
	}
	
}