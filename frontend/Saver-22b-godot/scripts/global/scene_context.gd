extends Node

#var villages_json_string := """{
	#"villages": [
	  #{
		#"id": 1,
		#"name": "평범한 도시 1",
		#"width": 3,
		#"height": 11,
		#"worldX": 5,
		#"worldY": 1,
		#"houses": [
		  #{
			#"x": 1,
			#"y": 1,
			#"owner": "0x53103C2D7875D2f5f02AeC3075155e268a6e3A94"
		  #}
		#]
	  #},
	  #{
		#"id": 2,
		#"name": "외로운 섬",
		#"width": 7,
		#"height": 13,
		#"worldX": -2,
		#"worldY": 7,
		#"houses": []
	  #},
	  #{
		#"id": 3,
		#"name": "정글",
		#"width": 3,
		#"height": 5,
		#"worldX": 1,
		#"worldY": -3,
		#"houses": []
	  #}
	#]
  #}"""
var villages: Array
var selected_village_index := 0
var user_state: Dictionary

#func _ready():
	#var json = JSON.new()
	#var error = json.parse(villages_json_string)
	#if error != OK:
		#print(error)
	#else:
		#villages = json.data.villages

func set_villages(query_data: Dictionary):
	villages = query_data.data.villages

func set_user_state(query_data: Dictionary):
	user_state = query_data.data.userState

func get_selected_village():
	return villages[selected_village_index]
