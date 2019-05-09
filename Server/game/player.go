package game

import (
	"encoding/json"
	"math/rand"
	"fmt"
	"./zero"
)

// Player Player
type Player struct {
	PlayerID string        `json:"playerID"`
	X        float64       `json:"x"`
	Y        float64       `json:"y"`
    W        float64       `json:"w"`
	Name     string        `json:"name"`
	Kills     int32        `json:"kills"`
	Xuanshang int32        `json:"xuanshang"`
	Session  *zero.Session `json:"-"`
	RoomNumber int32       `json:"roomnumber"`
	SkillLevel int       `json:"skilllevel"`
	
}

// CreatePlayer 创建玩家
func CreatePlayer(name string ,id string, s *zero.Session, room int32, x float64, y float64) *Player {

	player := &Player{
		PlayerID: id,
		Name:     name,
		X:        x,
		Y:        y,
        W:        rand.Float64()*100.0,
		Kills:    0,
		Xuanshang: 0,
		Session:  s,
		RoomNumber: room,
		SkillLevel: 1,
	}
	fmt.Println(player.Name + player.PlayerID)

	return player
}

// ToJSON 转成json数据
func (p *Player) ToJSON() []byte {
	b, _ := json.Marshal(p)
	return b
}
