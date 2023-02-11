using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadialBuildMenu : MonoBehaviour
{
    

    public GameObject SelectedWorkerUnit;
    // Rference to Radial Menu
    [SerializeField] GameObject RadialMenu;

    //reference to Move input
    Vector2 moveInput;

    // References to external options
    [SerializeField] GameObject[] options;
    [SerializeField] GameObject exitButton;
    [SerializeField] int optionSelected;
    // 360 / number of options 
    [SerializeField] float optionsThreshold;

    //colours for text
    [SerializeField] Color standard, highlight;
    [Header("Center Text")]
    [SerializeField] TMP_Text structureSelectedText;
    [SerializeField] Image selectedIcon;

    

    public void OpenRadialMenu()
    {
        RadialMenu.SetActive(true);
        //CheckOption(); 
    }

    public void CloseRadialMenu()
    {
        SelectObjectScript.Instance.SetModeToSelect();
        RadialMenu.SetActive(false);
    }

    void CheckOption(WorkerUnit.StructureType option)
    {
        if (SelectedWorkerUnit.GetComponent<WorkerUnit>().CheckIfSuitablePlaceForStructure(option))
        {
            PlaceStructure(WorkerUnit.StructureType.Outpost);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (RadialMenu.activeInHierarchy)
        {
            moveInput.x = Input.mousePosition.x - (Screen.width / 2f);
            moveInput.y = Input.mousePosition.y - (Screen.height / 2f);

            float _distance = Vector2.Distance(Vector2.zero, moveInput);
            //Debug.Log(_distance);

            moveInput.Normalize();



            if (moveInput != Vector2.zero)
            {


                float _angle = Mathf.Atan2(moveInput.y, -moveInput.x) / Mathf.PI;
                _angle *= 180f;
                _angle -= 90f;
                if (_angle < 0)
                {
                    _angle += 360;
                }

                //Debug.Log(_angle);

                for (int i = 0; i < options.Length; i++)
                {
                    //segments are 360 / number of options 
                    if (_angle > i * optionsThreshold && _angle < (i + 1) * optionsThreshold && _distance > 90 && _distance < 220)
                    {
                        //select option
                        optionSelected = i;

                        if(i == 2)
                        {
                            options[2].GetComponent<RadialOptionScript>().HighlightSegment(highlight);
                            structureSelectedText.text = "Cancel";
                            structureSelectedText.color = Color.white;

                            selectedIcon.sprite = options[2].GetComponent<RadialOptionScript>().icon.sprite;
                            selectedIcon.color = new Color(1, 1, 1, 0.7f);
                        }
                        else
                        {
                            WorkerUnit.StructureType _placeable = WorkerUnit.StructureType.Headquarters;

                            switch (optionSelected)
                            {
                                //Outpost
                                case 0:
                                    _placeable = WorkerUnit.StructureType.Outpost;
                                    break;
                                //Factory
                                case 1:
                                    _placeable = WorkerUnit.StructureType.Factory;
                                    break;
                                //Cancel Button
                                case 2:
                                    _placeable = WorkerUnit.StructureType.Headquarters;
                                    break;
                                //Power Generator
                                case 3:
                                    _placeable = WorkerUnit.StructureType.PowerCell;
                                    break;
                                //Research
                                case 4:
                                    _placeable = WorkerUnit.StructureType.Research;
                                    break;
                            }

                            options[i].GetComponent<RadialOptionScript>().HighlightSegment(highlight);
                            structureSelectedText.text = "" + options[i].GetComponent<RadialOptionScript>().unitName;
                            selectedIcon.sprite = options[i].GetComponent<RadialOptionScript>().icon.sprite;

                            if (SelectedWorkerUnit.GetComponent<WorkerUnit>().CheckIfSuitablePlaceForStructure(_placeable))
                            {
                                structureSelectedText.color = Color.white;
                                selectedIcon.color = new Color(1, 1, 1, 0.7f);
                            }
                            else
                            {
                                structureSelectedText.color = Color.red;
                                selectedIcon.color = Color.red;
                            } 
                        }
                    }
                    else // Cancel Option
                    {
                        options[i].GetComponent<RadialOptionScript>().UnHighlightSegment(standard);

                        if (_distance < 90 || _distance > 220)
                        {
                            structureSelectedText.text = "Cancel";
                            structureSelectedText.color = Color.white;

                            selectedIcon.sprite = options[2].GetComponent<RadialOptionScript>().icon.sprite;
                            selectedIcon.color = new Color(1, 1, 1, 0.7f);
                        }
                    }
                }

                
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (_distance < 90 || _distance > 220)
                {

                    SelectObjectScript.Instance.canSelect = true;
                    CloseRadialMenu();
                }
                else
                {
                    switch (optionSelected)
                    {
                        case 0: // Outpost 
                            PlaceStructure(WorkerUnit.StructureType.Outpost);

                            break;
                        case 1: // Factory
                            PlaceStructure(WorkerUnit.StructureType.Factory);

                            break;
                        case 2: // Cancel 
                            CloseRadialMenu();
                            break;
                        case 3: // PowerCell
                            PlaceStructure(WorkerUnit.StructureType.PowerCell);

                            break;
                        case 4: // Research
                            PlaceStructure(WorkerUnit.StructureType.Research);

                            break;
                    }
                }
            }
        }
    }

    public void PlaceStructure(WorkerUnit.StructureType _structureIndex)
    {
        if (SelectedWorkerUnit.GetComponent<WorkerUnit>().CheckIfSuitablePlaceForStructure(_structureIndex))
        {
            GameObject _placeable = Instantiate(SelectedWorkerUnit.GetComponent<WorkerUnit>().PlaceableStructures[(int)_structureIndex]);
            _placeable.GetComponent<StructureInfo>().owner = GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn];
            _placeable.transform.position = SelectedWorkerUnit.transform.position;
            _placeable.GetComponent<StructureInfo>().occupiedTile = SelectedWorkerUnit.GetComponent<UnitInfo>().occuipedTile; 
            _placeable.transform.parent = null;
            _placeable.GetComponent<StructureInfo>().UpdatePlayerDetails();
            Destroy(SelectedWorkerUnit.gameObject);
            CloseRadialMenu();
        }
        else
        {
            TileAudioManager.instance.PlayTileAudio(tileAudioType.negflip);
            Debug.Log("Can't Build that here!");
            
        }

        
    }
}
