﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickManager : MonoBehaviour {

	public GameObject selectTowerPannel;
	public GameObject TowerStatusPannel;
	public Text ErrMessage;
	
	void Update () {
		if (Input.GetMouseButtonDown (0) ) {
			//ErrMessage.GetComponent<ErrMessage>().show_message("test!!");
			check_open_selectTwoerPannel(); // 檢查要不要開啟蓋塔UI
		}
	}

	bool check_other_ui_panel(){
		Vector2 check = Input.mousePosition;
		//Debug.Log (check);
		if (check.x > 833 && check.y > 456)
			return false;

		if (check.x < 121 && check.y > 460)
			return false;

		return true;
	}

	void check_open_selectTwoerPannel(){
		if (check_other_ui_panel ()) { // 檢查有沒有點到其他UI
			if ( !anyPanelOpen() ) {
				check_raycast_to_open_panel ();
			} else {  
				float offset_x = 70;
				float offset_y = 140;
				Vector2 start = new Vector2 (selectTowerPannel.transform.position.x - offset_x, selectTowerPannel.transform.position.y + offset_y);
				Vector2 end = new Vector2 (selectTowerPannel.transform.position.x + offset_x, selectTowerPannel.transform.position.y - offset_y);
				Vector2 check = Input.mousePosition;
				if ((check.x > start.x && check.x < end.x) && (check.y > end.y || check.y < start.y)) {
					// do nothing
				} else {
					Debug.Log ("check:"+check+" end:"+end);
					check_raycast_to_open_panel ();
				}
			}
		}
	}

	void check_raycast_to_open_panel (){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if( Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("CannonPlatform") ) ){
			CannonPlatform selectCannon = hit.transform.gameObject.GetComponent<CannonPlatform>();
			//Debug.Log (selectCannon);
			if( selectCannon ){
				if( selectCannon.IsEmpty ){
					openBuildTowerPanel(selectCannon);
				} else {
					openTowerStatusPanel(selectCannon);
				}
			}
		}
	}

	Vector3 getPanelPosition(){
		Vector3 newPosition = Input.mousePosition;
		newPosition.x += 64.0F;
		newPosition.y -= 110.0F;
		
		if( newPosition.x > 776 ){
			newPosition.x -= 128.0F;
		}
		if( newPosition.y < 120 ){
			newPosition.y += 220.0F;
		}

		return newPosition;
	}

	bool anyPanelOpen (){
		if (selectTowerPannel.activeSelf)
			return true;

		if (TowerStatusPannel.activeSelf)  
			return true;

		return false;
	}

	void closeAllPanel() {
		selectTowerPannel.SetActive(false);
		TowerStatusPannel.SetActive(false);
	}

	void openTowerStatusPanel (CannonPlatform selectCannon){
		closeAllPanel ();
		TowerStatusPannel.GetComponent<TowerStatusManager>().set_useCannon(selectCannon);
		Vector3 newPosition = getPanelPosition();
		TowerStatusPannel.transform.position = newPosition;
		TowerStatusPannel.SetActive(true);
	}


	void openBuildTowerPanel (CannonPlatform selectCannon){
		closeAllPanel ();
		selectTowerPannel.GetComponent<TowerManager>().set_useCannon(selectCannon);
		Vector3 newPosition = getPanelPosition();
		selectTowerPannel.transform.position = newPosition;
		selectTowerPannel.SetActive(true);

	}
}
