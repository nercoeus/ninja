package game

import (
	"encoding/json"
)

type Death struct {
	AnemyID string           `json:"anemyID"`
	PlayerID string          `json:"playerID"`
	DeathType string         `json:"type"`
	Numbers int32      `json:"numbers"`
}

// ToJSON 转成json数据
func (p *Death) ToJSON() []byte {
	b, _ := json.Marshal(p)
	return b
}