package main

import (
	"log"
	"time"

	"./game"
	"./game/zero"
)
  //server main logic
func main() {
	host := ":9999"    //set point number

	ss, err := zero.NewSocketService(host)  //init server
	if err != nil {
		log.Println(err)
		return
	}

	ss.SetHeartBeat(5*time.Second, 30*time.Second)
//初始化操作函数,这里仅仅是吧game中定义的函数绑定而已...
	ss.RegMessageHandler(game.HandleMessage)
	ss.RegConnectHandler(game.HandleConnect)
	ss.RegDisconnectHandler(game.HandleDisconnect) 
//开始运行服务器
	log.Println("server running on " + host)
	ss.Serv()
}
