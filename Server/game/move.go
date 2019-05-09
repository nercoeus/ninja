package game

import (
	"encoding/json"
)

type Move struct {
	PlayerID int        `json:"playerID"`
	X        float64       `json:"x"`
	Y        float64       `json:"y"`
    W        float64       `json:"w"`
}

// ToJSON 转成json数据
func (p *Move) ToJSON() []byte {
	b, _ := json.Marshal(p)
	return b
}