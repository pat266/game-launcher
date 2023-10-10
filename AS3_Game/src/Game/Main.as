package Game
{
	import flash.display.Loader;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.KeyboardEvent;
	import flash.events.TimerEvent;
	import flash.utils.setTimeout;
	import flash.events.UncaughtErrorEvent;
	import flash.external.ExternalInterface;
	import flash.geom.Point;
	import flash.net.URLRequest;
	import flash.system.Security;
	import flash.system.fscommand;
	import flash.utils.ByteArray;
	import flash.utils.Timer;
	import flash.utils.getTimer;
	import flash.text.TextField;
	import flash.text.TextFieldType;
	import flash.display.Stage;
	import flash.display.StageScaleMode;
	import flash.display.StageAlign;
	import flash.system.System;



	public class Main extends Sprite
	{
		// các class của game
		
		private var NetWorkManager:Class;
		
		private var TSoundManager:Class;
		
		private var GameConfig:Class;
		
		private var TSocketEvent:Class;
		
		private var HeadResChange:Class;
		
		private var GameState:Class;
		
		private var LoginRoleVO:Class;
		
		private var ProcessManager:Class;
		
		private var FacadeManager:Class;
		
		private var PipeConstants:Class;
		
		private var SettingKey:Class;
		
		private var GameInstance:Class;
		
		private var UIInstance:Class;
		
		private var MainUI:Class;
		
		private var PipeManager:Class;
		
		private var SceneCharacterType:Class;
		
		private var SceneCharacter:Class;
		
		private var Scene:Class;
		
		private var CharStatusType:Class;
		
		private var Player:Class;
		
		private var NoticeManager:Class;
		
		private var MainCharSeachPathManager:Class;
		
		private var SceneConfig:Class;
		
		private var SkillInfo:Class;
		
		private var AfkInfo2:Class;
		
		private var GoodsInfo:Class;
		
		private var AttributeInfo:Class;
		
		// các biến cần dùng để xử lý của auto
		
		private var movie_url:String;
		
		private var is_active_auto:Boolean;
		
		private var user_id:String;

		private var server_id:int;
		
		private var char_name:String;
		
		private var square:int;
		
		private var flash_movie_loader:Loader;
		
		private var is_allow_create_auto_feature:Boolean;
		
		private var is_allow_auto:Boolean;
		
		private var is_allow_revive:Boolean;
		
		private var is_allow_ride_mount:Boolean;
		
		private var timer_allow_ride_mount:Timer;
		
		private var is_auto_start:Boolean;
		
		private var is_auto_revive:Boolean;
		
		private var timer_other_task:Timer;
		
		private var is_auto_ride_mount:Boolean;
		
		private var time_delay_auto_san_boss:int;
		
		private var timer_auto_san_boss:Timer;
		
		private var timer_auto_refresh_boss_status:Timer;
		
		private var time_delay_auto_use_goods:int;
		
		private var timer_auto_use_goods:Timer;
		
		private var boss_arr:Array;
		
		private var is_allow_san_boss:Boolean;
		
		private var auto_san_boss_type:int;
		
		private var is_allow_auto_use_goods:Boolean;
		
		private var auto_use_goods_name:String;
		
		private var timer_do_send_10051:Timer;

		public function Main()
		{
			is_active_auto = true;
			user_id = "";
			movie_url = "";
			server_id = 0;
			char_name = "";
			square = 25;
			is_allow_create_auto_feature = false;
			is_allow_auto = false;
			is_allow_revive = false;
			is_allow_ride_mount = true;
			timer_allow_ride_mount = new Timer(6000);
			is_auto_start = false;
			is_auto_revive = true;
			timer_other_task = new Timer(1000);
			is_auto_ride_mount = true;
			time_delay_auto_san_boss = 200;
			timer_auto_san_boss = new Timer(time_delay_auto_san_boss);
			timer_auto_refresh_boss_status = new Timer(5000);
			time_delay_auto_use_goods = 100;
			timer_auto_use_goods = new Timer(time_delay_auto_use_goods);
			boss_arr = [];
			is_allow_san_boss = false;
			auto_san_boss_type = 1;
			is_allow_auto_use_goods = false;
			auto_use_goods_name = "";
			timer_do_send_10051 = new Timer(60000);
			addEventListener(Event.ADDED_TO_STAGE, added_to_stage);
		}

		private function added_to_stage(e:Event):void
		{
			removeEventListener(Event.ADDED_TO_STAGE,added_to_stage);
			ExternalInterface.addCallback("loadMovie",_loadMovie);
			fscommand("loadMovie"); // báo cho ứng dụng thực hiện load movie
		}

		// TODO: Pass character name 
		private function _loadMovie(movie:String):void
		{
			char_name = "..."; // truyền tên đăng nhập vào đây nếu muốn bước sau tự động chọn nhân vật
			
			var url_request:URLRequest = new URLRequest(movie);
			Security.allowDomain("*");
			flash_movie_loader = new Loader();
			flash_movie_loader.contentLoaderInfo.addEventListener(Event.COMPLETE, flash_movie_loader_complete);
			this.movie_url = movie;
			flash_movie_loader.load(url_request);
			System.gc();
		}

		private function flash_movie_loader_complete(e:Event):void
		{
			flash_movie_loader.contentLoaderInfo.removeEventListener(Event.COMPLETE,flash_movie_loader_complete);
			flash_movie_loader.uncaughtErrorEvents.addEventListener(UncaughtErrorEvent.UNCAUGHT_ERROR,flash_movie_loader_uncaught_error_event);
			stage.addChild(flash_movie_loader.content);
			stage.addEventListener(Event.ENTER_FRAME, stage_enter_frame); // thêm sự kiện enter frame của stage để kiểm tra và load các class của game
			// add zoom in/out support callbacks
			zoomMap();
			zoomMenu();
			resetZoomDefault();
			
			// add restart the game support callbacks
			restartGame();
			
		}
		
		/**
		 * A function to add the ExternalInterface to accept call from outside (C#)
		 * Restart the game
		 */
		private function restartGame():void {
			var capturedMovieURL:String = this.movie_url;
			var self:Main = this; // Capture a reference to the current instance of Main
						
			ExternalInterface.addCallback("restartGame", function(text : String) : String {
				// Error handling
				try {
					// Step 1: Remove callbacks
					ExternalInterface.addCallback("restartGame", null);
					ExternalInterface.addCallback("zoomMap", null);
					ExternalInterface.addCallback("zoomMenu", null);
					ExternalInterface.addCallback("resetZoom", null);
					ExternalInterface.addCallback("loadMovie", null);

					// Step 2: Unload the current SWF
					if (self.stage) {
						self.stage.removeEventListener(Event.ENTER_FRAME, stage_enter_frame);
						if (self.flash_movie_loader && self.flash_movie_loader.content && self.stage.contains(self.flash_movie_loader.content)) {
							self.stage.removeChild(self.flash_movie_loader.content);
						}
					}
					if (self.flash_movie_loader) {
						self.flash_movie_loader.contentLoaderInfo.removeEventListener(Event.COMPLETE, flash_movie_loader_complete);
						self.flash_movie_loader.uncaughtErrorEvents.removeEventListener(UncaughtErrorEvent.UNCAUGHT_ERROR, flash_movie_loader_uncaught_error_event);
						self.flash_movie_loader.unloadAndStop(true);
						self.flash_movie_loader = null;
					}
					
					// stopping all of the timers
					// 1. Stop the timers
					timer_do_send_10051.stop();
					timer_auto_san_boss.stop();
					timer_auto_refresh_boss_status.stop();
					timer_auto_use_goods.stop();
					timer_allow_ride_mount.stop();
					timer_other_task.stop();

					// 2. Remove the event listeners
					timer_do_send_10051.removeEventListener(TimerEvent.TIMER, do_send_10051);
					timer_auto_san_boss.removeEventListener(TimerEvent.TIMER, auto_san_boss);
					timer_auto_refresh_boss_status.removeEventListener(TimerEvent.TIMER, auto_refresh_boss_status);
					timer_auto_use_goods.removeEventListener(TimerEvent.TIMER, auto_use_goods);
					timer_allow_ride_mount.removeEventListener(TimerEvent.TIMER, allow_ride_mount);
					timer_other_task.removeEventListener(TimerEvent.TIMER, other_task);

					get_stage()["removeEventListener"](KeyboardEvent.KEY_DOWN, stage_key_down);
					get_stage()["removeEventListener"](KeyboardEvent.KEY_UP, stage_key_up);
					
					NetWorkManager["lineSocket"]["removeEventListener"](TSocketEvent["CLOSE"], line_socket_close);
					get_ui()["loadingBar"]["removeEventListener"](Event.ADDED_TO_STAGE,loading_bar_added_to_stage); // thêm sự kiện xử lý màn hình load game
					get_ui()["loadingBar"]["removeEventListener"](Event.REMOVED_FROM_STAGE,loading_bar_remove_from_stage); // thêm sự kiện xử lý màn hình load game

					// 3. Nullify or reset the timer references
					timer_do_send_10051 = null;
					timer_auto_san_boss = null;
					timer_auto_refresh_boss_status = null;
					timer_auto_use_goods = null;
					timer_allow_ride_mount = null;
					timer_other_task = null;
					
					
					System.gc();
					// Step 3: Load the SWF again
					// setTimeout(_loadMovie, 2000, capturedMovieURL);
				} catch (error:Error) {
					trace("Error in restartGame: " + error.message);
				}
				return "";
			});
		}
		
		/**
		 * A function to add the ExternalInterface to accept call from outside (C#)
		 * Resizing the map requires a lot of processing to make sure the camera doesnt look too bad
		 */
		private function zoomMap():void {
			var self:Main = this; // Capture a reference to the current instance of Main
			ExternalInterface.addCallback("zoomMap", function(text : String) : String
			{
				if (self.stage && self.stage.numChildren > 0) {
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
			var self:Main = this; // Capture a reference to the current instance of Main
			ExternalInterface.addCallback("zoomMenu", function(text : String) : String
			{
				if (self.stage && self.stage.numChildren > 0) {
					var scale:Number = parseFloat(text);
					stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](2)["scaleX"] = scale;
					stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](2)["scaleY"] = scale;
				}
				
				return ""; // for testing purpose
			});
		}
		
		/**
		 * A function to add the ExternalInterface to accept call from outside (C#)
		 * Reset the zoom levels to default
		 */
		private function resetZoomDefault():void {
			var self:Main = this; // Capture a reference to the current instance of Main
			ExternalInterface.addCallback("resetZoom", function(text : String) : String {
				try {
					if (self.stage && self.stage.numChildren > 0) {
						self.stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](2)["scaleX"] = 1;
						self.stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](2)["scaleY"] = 1;

						self.stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](0)["scaleX"] = 1;
						self.stage["getChildAt"](1)["getChildAt"](0)["getChildAt"](0)["getChildAt"](0)["scaleY"] = 1;
					}
					return "Success"; // If everything went fine
				} catch (error:Error) {
					return "Error in resetZoom: " + error.message; // Return the error message to C#
				}
				return "Stage or its children are not initialized."; // Handle cases where the condition is false
			});
		}

		private function flash_movie_loader_uncaught_error_event(e:UncaughtErrorEvent):void
		{
			return;
		}

		private function stage_enter_frame(e:Event):void
		{
			// kiểm tra class NetWorkManager đã được khởi tạo hay chưa
			if (NetWorkManager == null)
			{
				if (has_definition("com.tgame.manager::NetWorkManager"))
				{
					NetWorkManager = get_definition("com.tgame.manager::NetWorkManager");
				}
				return;
			}
			// NetWorkManager đã đưcọ khởi tạo, xoá sự kiện enter frame của stage và load các class khác
			stage.removeEventListener(Event.ENTER_FRAME,stage_enter_frame);
			load_classes_part_1(); // load các class phần thứ nhất (các class cần thiết để xử lý các bước sau)
			TSoundManager["setBgSoundMute"](true);
			TSoundManager["setShortSoundMute"](true);
			// đăng ký các message cần thiết để xử lý các bước sau
			register_message(10012,received_10012,"received_10012"); // message đăng nhập thành công vào server
			register_message(10014,received_10014,"received_10014"); // message trả về thông tin các nhân vật
			register_message(10022,received_10022,"received_10022"); // message trả về thông tin của nhân vật đã chọn
		}

		private function has_definition(path:String):Boolean
		{
			return flash_movie_loader.contentLoaderInfo.applicationDomain.hasDefinition(path);
		}

		private function get_definition(path:String):Class
		{
			return flash_movie_loader.contentLoaderInfo.applicationDomain.getDefinition(path) as Class;
		}

		private function load_classes_part_1():void
		{
			if (has_definition("com.tgame.manager::TSoundManager"))
			{
				TSoundManager = get_definition("com.tgame.manager::TSoundManager");
			}
			if (has_definition("com.tgame.common::GameConfig"))
			{
				GameConfig = get_definition("com.tgame.common::GameConfig");
			}
			if (has_definition("com.tgame.common.staticdata::HeadResChange"))
			{
				HeadResChange = get_definition("com.tgame.common.staticdata::HeadResChange");
			}
			if (has_definition("com.tgame.common.vo.login::LoginRoleVO"))
			{
				LoginRoleVO = get_definition("com.tgame.common.vo.login::LoginRoleVO");
			}
			if (has_definition("com.tgame.common::GameState"))
			{
				GameState = get_definition("com.tgame.common::GameState");
			}
			if (has_definition("com.tgame.common.net.socket::TSocketEvent"))
			{
				TSocketEvent = get_definition("com.tgame.common.net.socket::TSocketEvent");
			}
			if (has_definition("com.tgame.manager::ProcessManager"))
			{
				ProcessManager = get_definition("com.tgame.manager::ProcessManager");
			}
			if (has_definition("com.tgame.manager::FacadeManager"))
			{
				FacadeManager = get_definition("com.tgame.manager::FacadeManager");
			}
			if (has_definition("com.tgame::PipeConstants"))
			{
				PipeConstants = get_definition("com.tgame::PipeConstants");
			}
		}

		private function register_message(message:int,func:Function,message_key:String):void
		{
			NetWorkManager["registerMsg"](message,func,message_key);
		}

		private function received_10012(message:Object):void
		{
			remove_message(10012,"received_10012"); // huỷ message 10012
			var data:ByteArray = get_data(message);
			reset_data_position(data);
			var login_success:int = data.readByte();
			if (login_success == 1)
			{
				var game_account:int = data.readInt();
				var is_chenmi:int = data.readByte();
				var login_auth:String = data.readUTF();
				var login_sign:String = data.readUTF();
				server_id = data.readInt(); // lấy thông tin server_id từ message
				user_id = data.readUTF(); // lấy thông tin user_id từ message
				var login_time:Number = data.readDouble();
			}
			reset_data_position(data);
		}
		
		private function remove_message(message:int,message_key:String):void
		{
			NetWorkManager["removeMsg"](message,message_key);
		}
		
		private function get_data(message:Object):ByteArray
		{
			return message["body"] as ByteArray;
		}
		
		private function reset_data_position(data:ByteArray):void
		{
			data.position = 0;
		}
		
		private function received_10014(message:Object):void
		{
			remove_message(10014,"received_10014"); // huỷ message 10014
			var data:ByteArray = get_data(message);
			reset_data_position(data);
			data.readByte() == 1;
			var char_count:int = data.readByte();
			var char_arr:Array = [];
			for (var i:int = 0; i < char_count; i++)
			{
				var char_id:int = data.readInt();
				var head_img:String = HeadResChange["headResChangeToString"](data.readByte());
				var char_name:String = data.readUTF();
				var char_level:int = data.readShort();
				var party_id:int = data.readByte();
				var vip_lv:int = data.readByte();
				var char_data:Object = set_char_data(char_id,head_img,char_name,char_level,party_id,vip_lv);
				char_arr.push(char_data); // đưa char_data vừa lấy được vào char_arr
			}
			reset_data_position(data);
			select_char(char_arr); // chọn nhân vật
			NetWorkManager["lineSocket"]["addEventListener"](TSocketEvent["CLOSE"],line_socket_close); // thêm sự kiện xử lý mất kết nối
		}
		
		private function line_socket_close(e:Object):void
		{
			fscommand("disconnected"); // báo cho ứng dụng biết là đã mất kết nối để xử lý
		}

		private function set_char_data(char_id:int,head_img:String,char_name:String,char_level:int,party_id:int,vip_lv:int):Object
		{
			return {"char_id":char_id,"head_img":head_img,"char_name":char_name,"char_level":char_level,"party_id":party_id,"vip_lv":vip_lv};
		}

		private function select_char(char_arr:Array):void
		{
			if (char_arr.length <= 0)
			{
				return;
			}
			// lấy dữ liệu nhân vật với char_name đã được truyền trước đó nếu có
			var char_data:Object = get_char_data(char_arr);
			if (char_data == null)
			{
				return;
			}
			// set thông tin nhân vật vào dữ liệu của game
			GameConfig["loginRoleVO"] = get_login_role_vo(char_data);
			// tiến hành các thao tác vào game
			GameState["hasSelectedChar"] = true;
			ProcessManager["enterLine"]();
			FacadeManager["killFacade"](PipeConstants["STARTUP_LOGIN"]);
		}

		private function get_char_data(char_arr:Array):Object
		{
			for (var i:int = 0; i < char_arr.length; i++)
			{
				if (char_arr[i]["char_name"] == char_name || char_arr[i]["char_name"] == get_full_char_name())
				{
					return char_arr[i];
				}
			}
			return null;
		}

		private function get_full_char_name():String
		{
			return "[" + server_id + "区]" + char_name;
		}

		private function get_login_role_vo(char_data:Object):Object
		{
			var login_role_vo:Object = new LoginRoleVO();
			login_role_vo["roleID"] = char_data["char_id"];
			login_role_vo["headImg"] = char_data["head_img"];
			login_role_vo["nickName"] = char_data["char_name"];
			login_role_vo["level"] = char_data["char_level"];
			login_role_vo["partyID"] = char_data["party_id"];
			login_role_vo["vipLV"] = char_data["vip_lv"];
			login_role_vo["sex"] = login_role_vo["getSex"]();
			return login_role_vo as LoginRoleVO;
		}
		
		private function received_10022(message:Object):void
		{
			remove_message(10022,"received_10022"); // huỷ message 10022
			var data:ByteArray = get_data(message);
			reset_data_position(data);
			var test_data:int = data.readInt();
			var char_id:int = data.readInt();
			var is_GM:Boolean = data.readByte() == 1;
			var char_name:String = data.readUTF();
			var head_img_type:int = data.readByte();
			var party_id:int = data.readByte();
			var party_tag:int = data.readByte();
			if (party_tag == 1)
			{
				var banghui_id:int = data.readInt();
				var banghui_name:String = data.readUTF();
			}
			reset_data_position(data);
			load_classes_part_2(); // load các class phần thứ 2 (các class cần thiết còn lại)
			get_ui()["loadingBar"]["addEventListener"](Event.ADDED_TO_STAGE,loading_bar_added_to_stage); // thêm sự kiện xử lý màn hình load game
			get_ui()["loadingBar"]["addEventListener"](Event.REMOVED_FROM_STAGE,loading_bar_remove_from_stage); // thêm sự kiện xử lý màn hình load game
			square = Math.floor(Math.sqrt(SceneConfig["TILE_WIDTH"] * SceneConfig["TILE_HEIGHT"]));
			// tắt một số các setting của game
			send_command(PipeConstants["SETTING_DATA"],[SettingKey["K6"],true]);
			send_command(PipeConstants["SETTING_DATA"],[SettingKey["K7"],true]);
			send_command(PipeConstants["SETTING_DATA"],[SettingKey["K2"],true]);
			send_command(PipeConstants["SETTING_DATA"],[SettingKey["K3"],true]);
			send_command(PipeConstants["SETTING_DATA"],[SettingKey["K4"],true]);
			send_command(PipeConstants["SETTING_DATA"],[SettingKey["K8"],true]);
			send_command(PipeConstants["SETTING_DATA"],[SettingKey["K12"],true]);
			// kiểm tra user_id nào được phép dùng auto
			if (user_id == "...")
			{
				is_active_auto = true;
			}
			is_allow_create_auto_feature = true; // cho phép tạo ra các tính năng cho auto
			create_auto_feature(); // tạo các tính năng cho auto
		}
		
		private function load_classes_part_2():void
		{
			if (has_definition("com.tgame.manager::PipeManager"))
			{
				PipeManager = get_definition("com.tgame.manager::PipeManager");
			}
			if (has_definition("com.tgame.common.staticdata::SettingKey"))
			{
				SettingKey = get_definition("com.tgame.common.staticdata::SettingKey");
			}
			if (has_definition("com.tgame.common::GameInstance"))
			{
				GameInstance = get_definition("com.tgame.common::GameInstance");
			}
			if (has_definition("com.zcp.engine::SceneCharacter"))
			{
				SceneCharacter = get_definition("com.zcp.engine::SceneCharacter");
			}
			if (has_definition("com.tgame.common.vo.character::Player"))
			{
				Player = get_definition("com.tgame.common.vo.character::Player");
			}
			if (has_definition("com.tgame.manager::NoticeManager"))
			{
				NoticeManager = get_definition("com.tgame.manager::NoticeManager");
			}
			if (has_definition("com.zcp.engine.staticdata::CharStatusType"))
			{
				CharStatusType = get_definition("com.zcp.engine.staticdata::CharStatusType");
			}
			if (has_definition("com.zcp.engine.staticdata::SceneCharacterType"))
			{
				SceneCharacterType = get_definition("com.zcp.engine.staticdata::SceneCharacterType");
			}
			if (has_definition("com.tgame.manager::MainCharSeachPathManager"))
			{
				MainCharSeachPathManager = get_definition("com.tgame.manager::MainCharSeachPathManager");
			}
			if (has_definition("com.zcp.engine.config::SceneConfig"))
			{
				SceneConfig = get_definition("com.zcp.engine.config::SceneConfig");
			}
			if (has_definition("com.tgame.common.vo.character.info::SkillInfo"))
			{
				SkillInfo = get_definition("com.tgame.common.vo.character.info::SkillInfo");
			}
			if (has_definition("com.tgame.common.vo.character.info::AfkInfo2"))
			{
				AfkInfo2 = get_definition("com.tgame.common.vo.character.info::AfkInfo2");
			}
			if (has_definition("com.tgame.common.vo.character.info::GoodsInfo"))
			{
				GoodsInfo = get_definition("com.tgame.common.vo.character.info::GoodsInfo");
			}
			if (has_definition("com.tgame.common.vo.character.info::AttributeInfo"))
			{
				AttributeInfo = get_definition("com.tgame.common.vo.character.info::AttributeInfo");
			}
			if (has_definition("com.zcp.engine::Scene"))
			{
				Scene = get_definition("com.zcp.engine::Scene");
			}
			if (has_definition("com.tgame.common.ui::UIInstance"))
			{
				UIInstance = get_definition("com.tgame.common.ui::UIInstance");
			}
			if (has_definition("com.tgame.shell.view.components::MainUI"))
			{
				MainUI = get_definition("com.tgame.shell.view.components::MainUI");
			}
		}
		
		private function get_ui():Object
		{
			return GameInstance["uiInstance"] as UIInstance;
		}
		
		private function loading_bar_added_to_stage(e:Event):void
		{
			is_allow_auto = false;
		}

		private function loading_bar_remove_from_stage(e:Event):void
		{
			is_allow_auto = true;
		}

		private function send_command(message:String,data:Object = null):void
		{
			PipeManager["sendMsg"](message,data);
		}
		
		private function create_auto_feature():void
		{
			if (! is_active_auto)
			{
				return;
			}
			if (! is_allow_create_auto_feature)
			{
				return;
			}
			is_allow_create_auto_feature = false;
			// đăng ký các message liên quan để xử lý
			register_message(20046,received_20046,"received_20046"); // message nhân vật die
			register_message(20078,received_20078,"received_20078"); // message nhân vật đếm thời gian chờ hồi sinh
			register_message(20076,received_20076,"received_20076"); // message nhân vật hồi sinh
			register_message(50048,received_50048,"received_50048"); // message nhân vật cưỡi thú
			register_message(40022, received_40022, "received_40022"); // message trả về danh sách boss
			timer_do_send_10051.addEventListener(TimerEvent.TIMER,do_send_10051); // thêm sự kiện cho timer xử lý gửi message 10051 sau một khoảng thời gian chỉ định
			timer_auto_san_boss.addEventListener(TimerEvent.TIMER,auto_san_boss); // thêm sự kiện cho timer xử lý quá trình săn boss
			timer_auto_refresh_boss_status.addEventListener(TimerEvent.TIMER,auto_refresh_boss_status); // thêm sự kiện cho timer refresh danh sách boss
			timer_auto_use_goods.addEventListener(TimerEvent.TIMER,auto_use_goods); // thêm sự kiện cho timer xử lý tính năng dùng vật phẩm
			timer_allow_ride_mount.addEventListener(TimerEvent.TIMER,allow_ride_mount); // thêm sự kiện cho timer xử lý tính năng cưỡi thú
			timer_other_task.addEventListener(TimerEvent.TIMER,other_task); // thêm sự kiện cho timer xử lý một số công việc khác
			get_stage()["addEventListener"](KeyboardEvent.KEY_DOWN,stage_key_down); // thêm sự kiện nhấn phím
			get_stage()["addEventListener"](KeyboardEvent.KEY_UP,stage_key_up); // thêm sự kiện nhả phím
		}
		
		private function do_send_10051(e:TimerEvent):void
		{
			if (! is_active_auto)
			{
				return;
			}
			if (! is_auto_start)
			{
				return;
			}
			if (! is_allow_auto)
			{
				return;
			}
			if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
			{
				send_10051(get_mainchar_location_x(),get_mainchar_location_y());
			}
		}
		
		private function auto_use_goods(e:TimerEvent):void
		{
			if (! is_active_auto)
			{
				return;
			}
			if (! is_allow_auto)
			{
				return;
			}
			if (! is_allow_auto_use_goods)
			{
				return;
			}
			if (auto_use_goods_name == "")
			{
				return;
			}
			for each(var goods in get_goods_bag_arr())
			{
				if ("[" + goods["res"]["name"] + "]" == auto_use_goods_name)
				{
					send_command(PipeConstants["SEND_ITEM_USE"],[goods["id"],goods["position"]]);
					return;
				}
			}
			notify("Không có vật phẩm " + auto_use_goods_name + "!",2);
			change_auto_use_goods_state();
		}
		
		private function get_goods_bag_arr():Array
		{
			return get_mainchar_goods_info()["goodsBagArr"] as Array;
		}
		
		private function get_mainchar_goods_info():Object
		{
			return get_mainchar_data()["goodsInfo"] as GoodsInfo;
		}
		
		private function received_40022(message:Object):void
		{
			var data:ByteArray = get_data(message);
			reset_data_position(data);
			boss_arr = [];
			var boss_count:int = data.readByte();
			for (var i:int = 0; i < boss_count; i++)
			{
				var boss_id:int = data.readInt();
				var boss_name:String = data.readUTF();
				var boss_grade:int = data.readShort();
				var boss_map:String = data.readUTF();
				var boss_time:String = data.readUTF();
				var boss_scene_id:int = data.readInt();
				var boss_x:int = data.readInt();
				var boss_y:int = data.readInt();
				var boss_say:int = data.readByte();
				var particular:String = data.readUTF();
				var pip:String = data.readUTF();
				var pip_id:int = data.readInt();
				var is_online:int = data.readByte();
				var blood:int = data.readByte();
				var skill:String = data.readUTF();
				if (boss_time != "未死亡")
				{
					continue;
				}
				if (boss_id == 1082 || boss_id == 1141 || boss_id == 1271 || boss_id == 1191 || boss_id == 1231 || boss_id == 1351 || boss_id == 1431 || boss_id == 1471 || boss_id == 1561 || boss_id == 1711 || boss_id == 1641 || boss_id == 1761 || boss_id == 1842 || boss_id == 1841 || boss_id == 2040 || boss_id == 63429 || boss_id == 2570)
				{
					boss_arr.push([boss_id,boss_name,boss_grade,boss_scene_id,boss_x,boss_y]);
				}
			}
			reset_data_position(data);
			is_allow_san_boss = true;
		}
		
		private function received_20046(message:Object):void
		{
			var data:ByteArray = get_data(message);
			reset_data_position(data);
			var char_id:int = data.readInt();
			reset_data_position(data);
			var target_char:Object = get_scene_char(char_id,SceneCharacterType["PLAYER"]);
			if (target_char == null || ! target_char["isMainChar"]())
			{
				return;
			}
			is_allow_revive = true;
		}
		
		private function get_scene_char(char_id:int,char_type:int):Object
		{
			return get_scene()["getCharByID"](char_id,char_type) as SceneCharacter;
		}
		
		private function get_scene():Object
		{
			return GameInstance["scene"] as Scene;
		}
		
		private function received_20078(message:Object):void
		{
			var data:ByteArray = get_data(message);
			reset_data_position(data);
			var char_id:int = data.readInt();
			var delay_revive_time:int = data.readByte();
			reset_data_position(data);
			var target_char:Object = get_scene_char(char_id,SceneCharacterType["PLAYER"]);
			if (target_char == null || ! target_char["isMainChar"]())
			{
				return;
			}
			is_allow_revive = false;
		}

		private function received_20076(message:Object):void
		{
			var data:ByteArray = get_data(message);
			reset_data_position(data);
			var char_id:int = data.readInt();
			reset_data_position(data);
			var target_char:Object = get_scene_char(char_id,SceneCharacterType["PLAYER"]);
			if (target_char == null || ! target_char["isMainChar"]())
			{
				return;
			}
			is_allow_revive = false;
		}

		private function received_50048(message:Object):void
		{
			var data:ByteArray = get_data(message);
			reset_data_position(data);
			var mount_id:int = data.readInt();
			var delay_ride_mount_time:int = data.readByte();
			reset_data_position(data);
			unallow_ride_mount();
			if (delay_ride_mount_time != 5)
			{
				timer_allow_ride_mount.delay = (delay_ride_mount_time + 1) * 1000;
			}
			timer_allow_ride_mount.start();
		}
		
		private function unallow_ride_mount():void
		{
			is_allow_ride_mount = false;
			timer_allow_ride_mount.reset();
		}
		
		private function allow_ride_mount(e:TimerEvent):void
		{
			timer_allow_ride_mount.reset();
			is_allow_ride_mount = true;
		}
		
		private function other_task(e:TimerEvent):void
		{
			if (! is_active_auto)
			{
				return;
			}
			if (! is_auto_start)
			{
				return;
			}
			if (! is_allow_auto)
			{
				return;
			}
			auto_revive();
			auto_ride_mount();
		}
		
		private function auto_revive():void
		{
			if (! is_auto_revive)
			{
				return;
			}
			if (! is_allow_revive)
			{
				return;
			}
			if (! is_death())
			{
				if (is_allow_revive)
				{
					is_allow_revive = false;
				}
				return;
			}
			if (get_mainchar_afk_info()["isStart"] && get_mainchar_afk_info()["isAutoUseFlower"] == 1)
			{
				return;
			}
			send_20075(); // gửi message hồi sinh
		}
		
		private function get_mainchar_afk_info():Object
		{
			return get_mainchar_data()["afkInfo2"] as AfkInfo2;
		}
		
		private function get_current_map_id():int
		{
			return get_scene()["mapConfig"]["mapID"];
		}
		
		private function is_death():Boolean
		{
			return get_mainchar()["getStatus"]() == CharStatusType["DEATH"];
		}
		
		private function get_mainchar_data():Object
		{
			return get_mainchar()["data"] as Player;
		}
		
		private function get_mainchar():Object
		{
			return GameInstance["mainChar"] as SceneCharacter;
		}

		private function send_20075():void
		{
			var data:ByteArray = new ByteArray();
			data.writeByte(0);
			data.writeDouble(getTimer());
			send_message(20075,data);
		}
		
		private function send_message(message:int,data:ByteArray):void
		{
			NetWorkManager["sendMsg"](message,data);
		}

		private function auto_ride_mount():void
		{
			if (! is_auto_ride_mount)
			{
				return;
			}
			if (! is_allow_ride_mount)
			{
				return;
			}
			if (is_death())
			{
				return;
			}
			if (get_mainchar()["isOnMount"])
			{
				return;
			}
			send_command(PipeConstants["UP_DOWN_HORSE"]);
		}
		
		private function stage_key_down(e:KeyboardEvent):void
		{
			if (! is_active_auto)
			{
				return;
			}
			if (! is_allow_auto)
			{
				return;
			}
			if (get_stage()["focus"] is TextField && get_stage()["focus"]["type"] == TextFieldType.INPUT)
			{
				return;
			}
			switch (e.keyCode)
			{
				case 37 :
					decrease_time_delay_auto_use_goods();
					return;
				case 38 :
					increase_time_delay_auto_san_boss();
					return;
				case 39 :
					increase_time_delay_auto_use_goods();
					return;
				case 40 :
					decrease_time_delay_auto_san_boss();
					return;
			}
		}
		
		private function get_stage():Object
		{
			return GameInstance["stage"] as Stage;
		}
		
		private function decrease_time_delay_auto_use_goods():void
		{
			if (time_delay_auto_use_goods <= 100)
			{
				return;
			}
			time_delay_auto_use_goods -= 100;
			timer_auto_use_goods.delay = time_delay_auto_use_goods;
			notify("Thời gian chờ tự động sử dụng vật phẩm: " + time_delay_auto_use_goods + "ms");
		}
		
		private function increase_time_delay_auto_use_goods():void
		{
			time_delay_auto_use_goods += 100;
			timer_auto_use_goods.delay = time_delay_auto_use_goods;
			notify("Thời gian chờ tự động sử dụng vật phẩm: " + time_delay_auto_use_goods + "ms");
		}
		
		private function decrease_time_delay_auto_san_boss():void
		{
			if (time_delay_auto_san_boss <= 1)
			{
				return;
			}
			time_delay_auto_san_boss--;
			timer_auto_san_boss.delay = time_delay_auto_san_boss;
			notify("Tốc độ săn BOSS: " + time_delay_auto_san_boss + "ms");
		}
		
		private function increase_time_delay_auto_san_boss():void
		{
			time_delay_auto_san_boss++;
			timer_auto_san_boss.delay = time_delay_auto_san_boss;
			notify("Tốc độ săn BOSS: " + time_delay_auto_san_boss + "ms");
		}
		
		private function stage_key_up(e:KeyboardEvent):void
		{
			if (! is_active_auto)
			{
				return;
			}
			switch (e.keyCode)
			{
				case 112:
					change_auto_use_goods_state();
					return;
				case 113 :
					change_auto_state();
					return;
				case 114:
					change_auto_san_boss_type();
					return;
			}
		}
		
		private function change_auto_use_goods_state():void
		{
			if (is_allow_auto_use_goods)
			{
				is_allow_auto_use_goods = false;
				auto_use_goods_name = "";
				timer_auto_use_goods.reset();
				notify("Tắt tự động sử dụng vật phẩm");
				return;
			}
			auto_use_goods_name = get_main_ui()["chatContainer"]["input"]["text"];
			if (auto_use_goods_name == "")
			{
				return;
			}
			is_allow_auto_use_goods = true;
			get_main_ui()["chatContainer"]["input"]["richText"] = "";
			timer_auto_use_goods.start();
			notify("Tự động sử dụng vật phẩm " + auto_use_goods_name);
		}
		
		private function get_main_ui():Object
		{
			return get_ui()["mainUI"] as MainUI;
		}
		
		private function change_auto_san_boss_type():void
		{
			switch(auto_san_boss_type)
			{
				case 1:
					auto_san_boss_type++;
					notify("Thứ tự săn BOSS: Giảm dần");
					return;
				case 2:
					auto_san_boss_type = 1;
					notify("Thứ tự săn BOSS: Tăng dần");
					return;
			}
		}
		
		private function change_auto_state():void
		{
			if (! is_auto_start)
			{
				boss_arr = [];
				is_allow_san_boss = false;
				send_40021();
				is_auto_start = true;
				timer_auto_san_boss.start();
				timer_auto_refresh_boss_status.start();
				timer_other_task.start();
				timer_do_send_10051.start();
				notify("Bật auto săn BOSS");
				return;
			}
			timer_other_task.reset();
			timer_auto_refresh_boss_status.reset();
			timer_auto_san_boss.reset();
			timer_do_send_10051.reset();
			is_auto_start = false;
			notify("Tắt auto săn BOSS");
		}
		
		private function notify(message:String,type:int = 1):void
		{
			NoticeManager["notify"](NoticeManager["SYSTEM_RIGHT"],message,false);
			switch (type)
			{
				case 1 :
					NoticeManager["notify"](NoticeManager["SYSTEM_CHAT"],message,false);
					return;
				case 2 :
					NoticeManager["notify"](NoticeManager["FOOT"],message,false);
					return;
			}
		}
		
		private function auto_san_boss(e:TimerEvent):void
		{
			if (! is_active_auto)
			{
				return;
			}
			if (! is_auto_start)
			{
				return;
			}
			if (! is_allow_auto)
			{
				return;
			}
			// kiểm tra và nhặt đồ
			if (check_pickup())
			{
				return;
			}
			if (! is_allow_san_boss)
			{
				return;
			}
			// kiểm tra kênh và đổi kênh
			if (GameConfig["lineID"] != 2)
			{
				if (! get_mainchar_attribute_info()["isVIP"] && get_current_map_id() >= 200212 && get_current_map_id() <= 200214)
				{
					out_map_200212();
					return;
				}
				if (get_current_map_id() != 20002)
				{
					go_to_map_20002();
					return;
				}
				send_10111(2); // gửi message đổi sang kênh 2
				return;
			}
			// về thành nếu không có boss
			if (boss_arr.length <= 0)
			{
				if (! get_mainchar_attribute_info()["isVIP"] && get_current_map_id() >= 200212 && get_current_map_id() <= 200214)
				{
					out_map_200212();
					return;
				}
				if (get_current_map_id() != 20002)
				{
					go_to_map_20002();
				}
				return;
			}
			switch(auto_san_boss_type)
			{
				// săn boss theo thứ tự thấp -> cao
				case 1:
					san_boss(boss_arr[0][1],boss_arr[0][2],boss_arr[0][3],boss_arr[0][4],boss_arr[0][5]);
					return;
				// săn boss theo thứ tự cao -> thấp
				case 2:
					san_boss(boss_arr[boss_arr.length - 1][1],boss_arr[boss_arr.length - 1][2],boss_arr[boss_arr.length - 1][3],boss_arr[boss_arr.length - 1][4],boss_arr[boss_arr.length - 1][5]);
					return;
			}
		}
		
		private function check_pickup():Boolean
		{
			if (get_scene()["sceneCharacters"]["length"] <= 0)
			{
				return false;
			}
			var char_point:Point = get_char_location(get_mainchar());
			var target_item:Object = null;
			var min_distance:int = 999999999;
			for each(var scene_char:Object in get_scene()["sceneCharacters"])
			{
				try
				{
					if (scene_char["isMainChar"]() || ! scene_char["usable"])
					{
						continue;
					}
					if (scene_char["type"] != SceneCharacterType["BAG"])
					{
						continue;
					}
					var scene_char_point:Point = get_char_location(scene_char);
					var distance:int = calculate_distance(char_point,scene_char_point);
					if (target_item == null || distance < min_distance)
					{
						target_item = scene_char as SceneCharacter;
						min_distance = distance;
					}
				}
				catch (error:Error)
				{
					continue;
				}
			}
			if (target_item == null)
			{
				return false;
			}
			if (get_mainchar_location_x() != target_item["tile_x"] || get_mainchar_location_y() != target_item["tile_y"])
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(get_current_map_id(),target_item["tile_x"],target_item["tile_y"],false);
				}
				return true;
			}
			send_command(PipeConstants["PICK_UP"],target_item["id"]);
			return true;
		}
		
		private function go_to_map_20002():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20002);
				return;
			}
			walk(20002);
		}
		
		private function get_mainchar_attribute_info():Object
		{
			return get_mainchar_data()["attributeInfo"] as AttributeInfo;
		}
		
		private function san_boss(boss_name:String,boss_grade:int,boss_map:int,boss_x:int,boss_y:int):void
		{
			if (! get_mainchar_attribute_info()["isVIP"] && boss_map != 200214 && get_current_map_id() >= 200212 && get_current_map_id() <= 200214)
			{
				out_map_200212();
				return;
			}
			if (get_current_map_id() != boss_map)
			{
				switch(boss_map)
				{
					case 20160:
						go_to_map_20160();
						return;
					case 20166:
						go_to_map_20166();
						return;
					case 20164:
						go_to_map_20164();
						return;
					case 20173:
						go_to_map_20173();
						return;
					case 20172:
						go_to_map_20172();
						return;
					case 20165:
						go_to_map_20165();
						return;
					case 20163:
						go_to_map_20163();
						return;
					case 20168:
						go_to_map_20168();
						return;
					case 20167:
						go_to_map_20167();
						return;
					case 20174:
						go_to_map_20174();
						return;
					case 20169:
						go_to_map_20169();
						return;
					case 20161:
						go_to_map_20161();
						return;
					case 20162:
						go_to_map_20162();
						return;
					case 20171:
						go_to_map_20171();
						return;
					case 20214:
						go_to_map_20214();
						return;
					case 20035:
						go_to_map_20035();
						return;
					case 20283:
						go_to_map_20283();
						return;
					default:
						notify("Chưa hỗ trợ săn BOSS " + boss_name + " (LV " + boss_grade + ")!",2);
						return;
				}
				return;
			}
			var char_point:Point = get_char_location(get_mainchar());
			var target_char:Object = null;
			var min_distance:int = 999999999;
			for each(var scene_char:Object in get_scene()["sceneCharacters"])
			{
				try
				{
					if (scene_char["isMainChar"]() || ! scene_char["usable"])
					{
						continue;
					}
					if (scene_char["getStatus"]() != CharStatusType["DEATH"] && scene_char["name"] == boss_name)
					{
						var scene_char_point:Point = get_char_location(scene_char);
						var distance:int = calculate_distance(char_point,scene_char_point);
						if (target_char == null || distance < min_distance)
						{
							target_char = scene_char as SceneCharacter;
							min_distance = distance;
						}
					}
				}
				catch (error:Error)
				{
					continue;
				}
			}
			var boss_point = new Point(boss_x,boss_y);
			if (calculate_distance(char_point,boss_point) <= 1000 && target_char != null && target_char["data"]["attributeInfo"]["hpNow"] != 0)
			{
				send_command(PipeConstants["CHANGED_LOCKEDCHAR_BASE_ATTR"],target_char);
				if (get_mainchar_skill_info()["mainSkill"] == null)
				{
					notify("Hãy chọn một kỹ năng để sử dụng!",2);
					return;
				}
				send_10083(get_mainchar_skill_info()["mainSkill"]["id"],target_char["type"],target_char["id"]); // gửi message đánh boss
				return;
			}
			if (get_mainchar_location_x() != boss_x || get_mainchar_location_y() != boss_y)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(boss_map,boss_x,boss_y);
				}
			}
		}
		
		private function out_map_200212():void
		{
			if (get_current_map_id() != 20212)
			{
				walk(200212);
				return;
			}
			if (get_mainchar()["tile_x"] != 27 || get_mainchar()["tile_y"] != 135)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20212,27,135);
				}
				return;
			}
			send_30001(1775,20002);
		}
		
		private function send_30001(npc_id:int,map_id:int):void
		{
			var data:ByteArray = new ByteArray();
			data.writeInt(npc_id);
			data.writeInt(map_id);
			send_message(30001,data);
		}
		
		private function get_mainchar_skill_info():Object
		{
			return get_mainchar_data()["skillInfo"] as SkillInfo;
		}
		
		private function go_to_map_20160():void
		{
			if (get_current_map_id() != 20003)
			{
				go_to_map_20003();
				return;
			}
			if (get_mainchar_location_x() != 21 || get_mainchar_location_y() != 146)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20003,21,146);
				}
				return;
			}
			send_10051(21,146);
		}
		
		private function go_to_map_20003():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20003);
				return;
			}
			walk(20003);
		}
		
		private function go_to_map_20166():void
		{
			if (get_current_map_id() != 20004)
			{
				go_to_map_20004();
				return;
			}
			if (get_mainchar_location_x() != 14 || get_mainchar_location_y() != 106)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20004,14,106);
				}
				return;
			}
			send_10051(14,106);
		}
		
		private function go_to_map_20004():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20004);
				return;
			}
			walk(20004);
		}
		
		private function go_to_map_20164():void
		{
			if (get_current_map_id() != 20005)
			{
				go_to_map_20005();
				return;
			}
			if (get_mainchar_location_x() != 63 || get_mainchar_location_y() != 104)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20005,63,104);
				}
				return;
			}
			send_10051(63,104);
		}
		
		private function go_to_map_20005():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20005);
				return;
			}
			walk(20005);
		}
		
		private function go_to_map_20173():void
		{
			if (get_current_map_id() != 20006)
			{
				go_to_map_20006();
				return;
			}
			if (get_mainchar_location_x() != 10 || get_mainchar_location_y() != 73)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20006,10,73);
				}
				return;
			}
			send_10051(10,73);
		}
		
		private function go_to_map_20006():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20006);
				return;
			}
			walk(20006);
		}
		
		private function go_to_map_20172():void
		{
			if (get_current_map_id() != 20009)
			{
				go_to_map_20009();
				return;
			}
			if (get_mainchar_location_x() != 13 || get_mainchar_location_y() != 34)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20009,13,34);
				}
				return;
			}
			send_10051(13,34);
		}
		
		private function go_to_map_20009():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20009);
				return;
			}
			walk(20009);
		}
		
		private function go_to_map_20165():void
		{
			if (get_current_map_id() != 20007)
			{
				go_to_map_20007();
				return;
			}
			if (get_mainchar_location_x() != 139 || get_mainchar_location_y() != 24)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20007,139,24);
				}
				return;
			}
			send_10051(139,24);
		}
		
		private function go_to_map_20007():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20007);
				return;
			}
			walk(20007);
		}
		
		private function go_to_map_20163():void
		{
			if (get_current_map_id() != 20008)
			{
				go_to_map_20008();
				return;
			}
			if (get_mainchar_location_x() != 29 || get_mainchar_location_y() != 103)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20008,29,103);
				}
				return;
			}
			send_10051(29,103);
		}
		
		private function go_to_map_20008():void
		{
			if (get_current_map_id() != 20007)
			{
				go_to_map_20007();
				return;
			}
			if (get_mainchar_location_x() != 14 || get_mainchar_location_y() != 130)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20007,14,130);
				}
				return;
			}
			send_10051(14,130);
		}
		
		private function go_to_map_20168():void
		{
			if (get_current_map_id() != 20010)
			{
				go_to_map_20010();
				return;
			}
			if (get_mainchar_location_x() != 141 || get_mainchar_location_y() != 42)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20010,141,42);
				}
				return;
			}
			send_10051(141,42);
		}
		
		private function go_to_map_20010():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20010);
				return;
			}
			walk(20010);
		}
		
		private function go_to_map_20167():void
		{
			if (get_current_map_id() != 20011)
			{
				go_to_map_20011();
				return;
			}
			if (get_mainchar_location_x() != 25 || get_mainchar_location_y() != 20)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20011,25,20);
				}
				return;
			}
			send_10051(25,20);
		}
		
		private function go_to_map_20011():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20011);
				return;
			}
			walk(20011);
		}
		
		private function go_to_map_20174():void
		{
			if (get_current_map_id() != 20013)
			{
				go_to_map_20013();
				return;
			}
			if (get_mainchar_location_x() != 141 || get_mainchar_location_y() != 15)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20013,141,15);
				}
				return;
			}
			send_10051(141,15);
		}
		
		private function go_to_map_20013():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20013);
				return;
			}
			walk(20013);
		}
		
		private function go_to_map_20169():void
		{
			if (get_current_map_id() != 20012)
			{
				go_to_map_20012();
				return;
			}
			if (get_mainchar_location_x() != 35 || get_mainchar_location_y() != 13)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20012,35,13);
				}
				return;
			}
			send_10051(35,13);
		}
		
		private function go_to_map_20012():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20012);
				return;
			}
			walk(20012);
		}
		
		private function go_to_map_20161():void
		{
			if (get_current_map_id() != 20014)
			{
				go_to_map_20014();
				return;
			}
			if (get_mainchar_location_x() != 36 || get_mainchar_location_y() != 106)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20014,36,106);
				}
				return;
			}
			send_10051(36,106);
		}
		
		private function go_to_map_20014():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20014);
				return;
			}
			walk(20014);
		}
		
		private function go_to_map_20162():void
		{
			if (get_current_map_id() != 20015)
			{
				go_to_map_20015();
				return;
			}
			if (get_mainchar_location_x() != 133 || get_mainchar_location_y() != 23)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20015,133,23);
				}
				return;
			}
			send_10051(133,23);
		}
		
		private function go_to_map_20015():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20015);
				return;
			}
			walk(20015);
		}
		
		private function go_to_map_20171():void
		{
			if (get_current_map_id() != 20015)
			{
				go_to_map_20015();
				return;
			}
			if (get_mainchar_location_x() != 143 || get_mainchar_location_y() != 153)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20015,143,153);
				}
				return;
			}
			send_10051(143,153);
		}
		
		private function go_to_map_20214():void
		{
			if (get_current_map_id() != 20213)
			{
				go_to_map_20213();
				return;
			}
			if (get_mainchar_location_x() != 127 || get_mainchar_location_y() != 24)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20213,127,24);
				}
				return;
			}
			send_10051(127,24);
		}
		
		private function go_to_map_20213():void
		{
			if (get_current_map_id() != 20212)
			{
				go_to_map_20212();
				return;
			}
			if (get_mainchar_location_x() != 40 || get_mainchar_location_y() != 19)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20212,40,19);
				}
				return;
			}
			send_10051(40,19);
		}
		
		private function go_to_map_20212():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20212);
				return;
			}
			if (get_current_map_id() != 20002)
			{
				go_to_map_20002();
				return;
			}
			if (get_mainchar_location_x() != 144 || get_mainchar_location_y() != 139)
			{
				if (get_mainchar()["getStatus"]() != CharStatusType["WALK"])
				{
					walk(20002,144,139);
				}
				return;
			}
			send_30001(1656,20212);
		}
		
		private function go_to_map_20035():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20035);
				return;
			}
			walk(20035);
		}
		
		private function go_to_map_20283():void
		{
			if (get_current_map_id() != 20280)
			{
				go_to_map_20280();
				return;
			}
			send_70567();
		}
		
		private function go_to_map_20280():void
		{
			if (get_mainchar_attribute_info()["isVIP"])
			{
				send_10133(20280);
				return;
			}
			walk(20280);
		}
		
		private function send_70567():void
		{
			var data:ByteArray = new ByteArray();
			send_message(70567,data);
		}
		
		private function get_char_location(char:Object):Point
		{
			return new Point(char["tile_x"],char["tile_y"]);
		}
		
		private function get_mainchar_location_x():int
		{
			return get_mainchar()["tile_x"];
		}
		
		private function get_mainchar_location_y():int
		{
			return get_mainchar()["tile_y"];
		}
		
		private function calculate_distance(point_1:Point,point_2:Point):int
		{
			return Math.floor(Point.distance(point_1,point_2) * square);
		}
		
		private function send_10083(skill_id:int,target_char_type:int,target_char_id:int):void
		{
			var data:ByteArray = new ByteArray();
			data.writeInt(skill_id);
			data.writeByte(target_char_type);
			data.writeInt(target_char_id);
			data.writeDouble(getTimer());
			send_message(10083,data);
		}
		
		private function walk(map_id:int,tile_x:int = -1,tile_y:int = -1,show_auto_find_path_text:Boolean = true):void
		{
			MainCharSeachPathManager["mainCharWalk"](map_id + "," + tile_x + "," + tile_y + ",0",show_auto_find_path_text);
		}
		
		private function send_10051(tile_x:int,tile_y:int):void
		{
			var data:ByteArray = new ByteArray();
			data.writeShort(tile_x);
			data.writeShort(tile_y);
			send_message(10051,data);
		}
		
		private function send_10133(map_id:int):void
		{
			var data:ByteArray = new ByteArray();
			data.writeInt(map_id);
			data.writeDouble(getTimer());
			send_message(10133,data);
		}
		
		private function send_10111(line_id:int):void
		{
			var data:ByteArray = new ByteArray();
			data.writeByte(line_id);
			data.writeDouble(getTimer());
			send_message(10111,data);
		}
		
		private function auto_refresh_boss_status(e:TimerEvent):void
		{
			if (! is_active_auto)
			{
				return;
			}
			if (! is_auto_start)
			{
				return;
			}
			if (! is_allow_auto)
			{
				return;
			}
			send_40021();
		}
		
		private function send_40021():void
		{
			var data:ByteArray = new ByteArray();
			send_message(40021,data);
		}
	}
}