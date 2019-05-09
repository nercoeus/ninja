package game

import (
	"encoding/json"
	"log"
	"fmt"
	"strconv"
	"./zero"
)
//当前玩家编号
var players int = 0
//每个房间容纳人数
var number int = 4
//当前房间号
var NowRoomNumber int32 = 0
//获胜所需分数
var winnerNumber int32 = 15
//这里主要是处理接收到的消息
// HandleMessage 处理网络请求
func HandleMessage(s *zero.Session, msg *zero.Message) {
	msgID := msg.GetID()
//对收到的消息进行switch处理,这里仅仅进行前两种的操作的逻辑处理
	fmt.Println(msgID)
	switch msgID {
	case RequestJoin:  //请求处理
		if (players % number) == 0{
			//fmt.Println("new room")
			NowRoomNumber += 1
		}

		players+=1

		//id := s.GetConn().GetName()
		id := strconv.Itoa(players)
		var f interface{}
		err := json.Unmarshal(msg.GetData(), &f)
		if err != nil {
			return
		}

		//创建新的玩家
		data2 := f.(map[string]interface{})
		Name := data2["name"].(string)
		Xposition := data2["x"].(float64)
		Yposition := data2["y"].(float64)
		//fmt.Println("new player name: ", Name, " x,y: ", Xposition, " " ,Yposition)
		player := CreatePlayer(Name, id, s, NowRoomNumber, Xposition, Yposition)

		m := make(map[string]interface{})
		m["self"] = player
		m["list"] = world.GetPlayerList(NowRoomNumber)
		m["roomNumber"] = NowRoomNumber
		data, _ := json.Marshal(m)
		//处理加入逻辑
		
		//fmt.Println(players)
		response := zero.NewMessage(ResponseJoin, data)
		s.GetConn().SendMessage(response)
		//通知所有玩家新玩家加入
		for _, p := range world.GetPlayerList(NowRoomNumber) {
			response := zero.NewMessage(BroadcastJoin, player.ToJSON())
			p.Session.GetConn().SendMessage(response)
		}

		world.AddPlayer(player)

		s.BindUserID(player.PlayerID)

		//fmt.Println("hello new player")
		//通知玩家持续等待状态
		var waitData Death
		waitData.PlayerID = Name
		if players % number == 0 {
			waitData.DeathType = "OK"
		} else {
			waitData.DeathType = strconv.Itoa((players % number))
			//fmt.Println(waitData.DeathType)
		}
		playersList := world.GetPlayerList(player.RoomNumber)
		for _, p := range playersList {
			//fmt.Println("send msg")
			response := zero.NewMessage(BroadcastWaitBegin, waitData.ToJSON())
			p.Session.GetConn().SendMessage(response)
		}

		break

	case RequestMove:    //请求移动,这里是define中定义的几种基本的逻辑操作...
		var f interface{}
		err := json.Unmarshal(msg.GetData(), &f)
		if err != nil {
			return
		}
		m := f.(map[string]interface{})

		x := m["x"]
		y := m["y"]
        w := m["w"]
		playerID := m["playerID"]


		
		player := world.GetPlayer(strconv.Itoa(int(playerID.(float64))))
		var data Move
		data.PlayerID = int(playerID.(float64))
		data.X = float64(x.(float64))
		data.Y = float64(y.(float64))
        data.W = float64(w.(float64))
		
		players := world.GetPlayerList(player.RoomNumber)
		for _, p := range players {
			message := zero.NewMessage(BroadcastMove, data.ToJSON())
			p.Session.GetConn().SendMessage(message)
		}
		break
    case RequestDisappear:   //请求隐身
        var f interface{}
        err := json.Unmarshal(msg.GetData(), &f)
        if err != nil{
            return
        }
        m := f.(map[string]interface{})
        playerID := m["playerID"].(string)
        //fmt.Println(playerID)
        player := world.GetPlayer(playerID)
        players := world.GetPlayerList(player.RoomNumber)
        for _, p := range players {
            message := zero.NewMessage(BroadcastDisappear, player.ToJSON())
            p.Session.GetConn().SendMessage(message)
        }
        break
    case RequestDarts:
        var f interface{}
		err := json.Unmarshal(msg.GetData(), &f)
		if err != nil {
			return
		}
		m := f.(map[string]interface{})
		x := m["x"]
		y := m["y"]
        w := m["w"]
		level := m["skilllevel"]
		playerID := m["playerID"].(string)

		player := world.GetPlayer(playerID)
		player.X = float64(x.(float64))
		player.Y = float64(y.(float64))
        player.W = float64(w.(float64))
		player.SkillLevel = int(level.(float64))
        //fmt.Println(" darts: ",  player.X, player.Y ,player.W ,playerID, " ", level)
		players := world.GetPlayerList(player.RoomNumber)
		for _, p := range players {
			message := zero.NewMessage(BroadcastDarts, player.ToJSON())
			p.Session.GetConn().SendMessage(message)
		} 
        break;
    case RequestRound:
        var f interface{}
		err := json.Unmarshal(msg.GetData(), &f)
		if err != nil {
			return
		}
		m := f.(map[string]interface{})
		x := m["x"]
		y := m["y"]
		playerID := m["playerID"].(string)

		player := world.GetPlayer(playerID)
		player.X = float64(x.(float64))
		player.Y = float64(y.(float64))
        //fmt.Println( player.X, player.Y ,playerID)
		players := world.GetPlayerList(player.RoomNumber)
		for _, p := range players {
			message := zero.NewMessage(BroadcastRound, player.ToJSON())
			p.Session.GetConn().SendMessage(message)
		} 
        break;
	case RequestDeath:
        var f interface{}
		err := json.Unmarshal(msg.GetData(), &f)
		if err != nil {
			return
		}
		m := f.(map[string]interface{})
		AnemyID := m["anemyID"].(string)
		PlayerID := m["playerID"].(string)
		Type := m["deathType"].(string)


		enemy := world.GetPlayer(AnemyID)
		player := world.GetPlayer(PlayerID)
		
		var data Death
		//增加击杀悬赏
		if enemy != nil {
			enemy.Xuanshang += 1
			enemy.Kills += player.Xuanshang/2+1
			data.Numbers = enemy.Kills*1000+enemy.Xuanshang/2+1
		}else{
			break;
		}
		data.PlayerID = PlayerID
		data.AnemyID = AnemyID
		data.DeathType = Type

		//fmt.Println("numbers: " ,data.Numbers)

		players := world.GetPlayerList(player.RoomNumber)
		for _, p := range players {
			message := zero.NewMessage(BroadcastDeath, data.ToJSON())
			p.Session.GetConn().SendMessage(message)
		} 
		
		//添加分数系统
		
		//清空击杀悬赏
		player.Xuanshang = 0

		//fmt.Println( "death: " , enemy.Name, player.Name, enemy.Xuanshang)
		
		if enemy.Kills >= winnerNumber{
			var data2 Death
			data2.AnemyID = AnemyID
			for _, p := range players {
				message := zero.NewMessage(BroadcastCountDown, data2.ToJSON())
				p.Session.GetConn().SendMessage(message)
			} 
		}
		
		//if enemy.Kills >= winnerNumber{
		//	m2 := make(map[string]interface{})
		//	m2["self"] = enemy.Kills
		//	m2["list"] = world.GetPlayerList(NowRoomNumber)
		//	m2["enemyID"] = enemy.Name;
		//	data, _ := json.Marshal(m2)
		//	fmt.Println("game end, winner is :  " + enemy.Name)
		//	for _, p := range players {
		//		message := zero.NewMessage(BroadcastWinner, data)
		//		p.Session.GetConn().SendMessage(message)
		//	} 
		//}
		
		break;
	case RequestBlink:
        var f interface{}
		err := json.Unmarshal(msg.GetData(), &f)
		if err != nil {
			return
		}
		m := f.(map[string]interface{})
		x := m["x"]
		y := m["y"]
        w := m["w"]
		playerID := m["playerID"].(string)

		player := world.GetPlayer(playerID)
		player.X = float64(x.(float64))
		player.Y = float64(y.(float64))
        player.W = float64(w.(float64))

		players := world.GetPlayerList(player.RoomNumber)
		for _, p := range players {
			message := zero.NewMessage(BroadcastBlink, player.ToJSON())
			p.Session.GetConn().SendMessage(message)
		}
		break
	case RequestWait:   //请求隐身
        var f interface{}
        err := json.Unmarshal(msg.GetData(), &f)
        if err != nil{
            return
        }
        m := f.(map[string]interface{})
        playerID := m["playerID"].(string)
        //fmt.Println( "ｗａｉｔ：　",playerID)
        player := world.GetPlayer(playerID)
        players := world.GetPlayerList(player.RoomNumber)
        for _, p := range players {
            message := zero.NewMessage(BroadcastWait, player.ToJSON())
            p.Session.GetConn().SendMessage(message)
        }
		break
	case RequestSL:
        var f interface{}
		err := json.Unmarshal(msg.GetData(), &f)
		if err != nil {
			return
		}
		m := f.(map[string]interface{})
		x := m["x"]
		y := m["y"]

		w := m["w"]
		level := m["skilllevel"]
		playerID := m["playerID"].(string)

		player := world.GetPlayer(playerID)
		player.X = float64(x.(float64))
		player.Y = float64(y.(float64))
        player.W = float64(w.(float64))
		player.SkillLevel = int(level.(float64))
		//fmt.Println(" shenglong: ",  player.X, player.Y ,player.W ,playerID, " ", level)
		players := world.GetPlayerList(player.RoomNumber)
		for _, p := range players {
			message := zero.NewMessage(BroadcastSL, player.ToJSON())
			p.Session.GetConn().SendMessage(message)
		}
		break
	case RequestRe:
		var f interface{}
		err := json.Unmarshal(msg.GetData(), &f)
		if err != nil {
			return
		}
		m := f.(map[string]interface{})
		x := m["x"]
		y := m["y"]
		playerID := m["playerID"].(string)

		player := world.GetPlayer(playerID)
		player.X = float64(x.(float64))
		player.Y = float64(y.(float64))
		players := world.GetPlayerList(player.RoomNumber)
		for _, p := range players {
			message := zero.NewMessage(BroadcastRe, player.ToJSON())
			p.Session.GetConn().SendMessage(message)
		}
		break
	case RequestCaozhi:
        var f interface{}
		err := json.Unmarshal(msg.GetData(), &f)
		if err != nil {
			return
		}
		m := f.(map[string]interface{})
		x := m["x"]
		y := m["y"]
		w := m["w"]
		
		level := m["skilllevel"]
		playerID := m["playerID"].(string)
		player := world.GetPlayer(playerID)
		player.X = float64(x.(float64))
		player.Y = float64(y.(float64))
        player.W = float64(w.(float64))
		player.SkillLevel = int(level.(float64))
		//fmt.Println(" caozhi: ",  player.X, player.Y ,player.W ,playerID, " ", level)
		players := world.GetPlayerList(player.RoomNumber)
		for _, p := range players {
			message := zero.NewMessage(BroadcastCaozhi, player.ToJSON())
			p.Session.GetConn().SendMessage(message)
		}
		break
	case RequestWinner:
		var f interface{}
		err := json.Unmarshal(msg.GetData(), &f)
		if err != nil {
			return
		}
		m := f.(map[string]interface{})
		playerID := m["playerID"].(string)
		player := world.GetPlayer(playerID)

		m2 := make(map[string]interface{})
		m2["list"] = world.GetPlayerList(player.RoomNumber)
		m2["enemyID"] = player.Name;
		data, _ := json.Marshal(m2)
		//fmt.Println("game end, winner is :  " + player.Name)
		players := world.GetPlayerList(player.RoomNumber)
		for _, p := range players {
			message := zero.NewMessage(BroadcastWinner, data)
			p.Session.GetConn().SendMessage(message)
		} 
		
		break
	}
}

// HandleDisconnect 处理网络断线
func HandleDisconnect(s *zero.Session, err error) {
	log.Println(s.GetConn().GetName() + " lost.")
	uid := s.GetUserID()
	lostPlayer := world.GetPlayer(uid)
	if lostPlayer == nil {
		return
	}
	player := world.GetPlayer(uid)
	world.RemovePlayer(uid)
	for _, p := range world.GetPlayerList(player.RoomNumber) {
		message := zero.NewMessage(BroadcastLeave, lostPlayer.ToJSON())
		p.Session.GetConn().SendMessage(message)
	}
}

// HandleConnect 处理网络连接
func HandleConnect(s *zero.Session) {
	log.Println(s.GetConn().GetName() + " connected.")
}
 
