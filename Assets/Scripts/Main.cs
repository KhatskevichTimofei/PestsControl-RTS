﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour
{
    public static Main instance; //Создаётся публичная статичная переменнная типа Main. Static 

    public Vector3 startMouse;
    public Vector3 stopMouse;
    public Image image1;
    public List<Unit> allUnits = new List<Unit>();
    public List<Build> allBuild = new List<Build>();
    public List<ISelected> selected = new List<ISelected>();
    public List<ISelected> allSelectebleObjects = new List<ISelected>();
    public Dictionary<KeyCode, List<ISelected>> saveSelectedObject = new Dictionary<KeyCode, List<ISelected>>();
    public Button continueButton;
    public Printer3D mainBuild;
    bool isFrameSelected;

    void Start()
    {
        instance = this; //Переменной типа Main мы задаём здачения самого себя 
        allSelectebleObjects.AddRange(allUnits); //В список всех построек и юнитов записываются все юниты
        allSelectebleObjects.AddRange(allBuild); //В список всех построке и юнитов записываются все построек
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P))//При одном нажатии кнопки P выполняется условие
        //{

        //    selected.Clear(); //Проходит очистка списка выбранных юнитов
        //    selected.Add(allUnits[Random.Range(0, allUnits.Count)]); //Выбранные юниты берутся из списка всех существующих юнитов
        //}
        //if (Input.GetKeyDown(KeyCode.Y))//При одном нажатии кнопки Y выполняется условие
        //    mainBuild.CreateUnit(1);//MainBuild создаёт юнита с индексом 1 при нажатии кнопки Y 
        if (Input.GetMouseButtonDown(1)) //При нажатии правой кнопки выполняется условие 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Создаётся переменнная типа Ray(луч) ??????????
            RaycastHit casthit; // Слздаётся переменнная типа RaycastHit(???)

            if (Physics.Raycast(ray, out casthit)) // ????
            {
                for (int i = 0; i < selected.Count; i++)//Цикл который проходится по всем выбранным юнитам
                {
                    if (selected[i] as Unit != null)
                        (selected[i] as Unit).SetTargetPosition(casthit.point);//Каждый выбранный юнит обращается к выбранной позиции, которая задаётся с помощью луча 
                }
            }
            //    if (agent.CalculatePath(casthit.point, path))
            //    {
            //        agent.SetPath(path);
            //    }

        }
        if (Input.GetMouseButtonDown(0)) //При нажатии левой кнопки мыши выполняется условие
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                startMouse = Input.mousePosition; // В пеменную типа Vector3 записывается позиция мыши
                for (int i = 0; i < selected.Count; i++)
                {
                    selected[i].IsSelected = false;
                }
                selected.Clear();//Отчищается список выбранных юнитов 
                image1.enabled = true; //Включает картинку для выделения
                isFrameSelected = true;
            }
        }
        if (Input.GetMouseButton(0) && isFrameSelected) //При отжатии левой кнопки мыши выполняется условие
        {
            stopMouse = Input.mousePosition; //В переменную типа Vector3 записывается конечное значение координат нахождения мыши 
            image1.rectTransform.anchoredPosition = startMouse; //В переменную startMouse записывается значение нахождения объекта image1 на сцене. 
            Vector2 delta = stopMouse - startMouse; //В переменную записывается координаты от разности конечной точки положения мыши на экране и начальной точки положения мыши на экране 
            if (delta.x < 0) //Блок который делает координату x положительной и смещает её по ширине 
            {
                image1.rectTransform.anchoredPosition += new Vector2(delta.x, 0); //
                delta.x *= -1;
            }
            if (delta.y < 0) //Блок который делает координату y  положительной и смещает её по высоте
            {
                image1.rectTransform.anchoredPosition += new Vector2(0, delta.y);
                delta.y *= -1;
            }
            image1.rectTransform.sizeDelta = delta;// Картинке передаётся размер находящийся в переменной delta
        }
        if (Input.GetMouseButtonUp(0) && isFrameSelected) //Выделяет юнитов попавших в картинку
        {

            for (int i = 0; i < allSelectebleObjects.Count; i++) //Создаёт новое изображение и записываает его в перменную area. 
            {
                Rect area = new Rect(image1.rectTransform.anchoredPosition, image1.rectTransform.sizeDelta);
                if (area.Contains(Camera.main.WorldToScreenPoint((allSelectebleObjects[i] as MonoBehaviour).transform.position))) //Если в созданную картинку area попадает какой либо юнит, выполняется условие  и записывает из массива allUnits юнитов в массив selectedUnits 
                {

                    selected.Add(allSelectebleObjects[i]);
                    allSelectebleObjects[i].IsSelected = true;
                }
                image1.enabled = false;//Выключает картинку

            }
            isFrameSelected = false;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            for (int i = 48; i < 58; i++)
                if (Input.GetKeyDown((KeyCode)i))
                {
                    if (saveSelectedObject.ContainsKey((KeyCode)i))
                        saveSelectedObject.Remove((KeyCode)i);

                    saveSelectedObject.Add((KeyCode)i, new List<ISelected>(selected));
                }
        }
        else
            for (int i = 48; i < 58; i++)
                if (Input.GetKeyDown((KeyCode)i))
                    if (saveSelectedObject.ContainsKey((KeyCode)i))
                    {
                        for (int j = 0; j < selected.Count; j++)
                        {
                            selected[j].IsSelected = false;
                        }
                        selected.Clear();

                        selected.AddRange(saveSelectedObject[(KeyCode)i]);
                        for (int j = 0; j < selected.Count; j++)
                        {
                            selected[j].IsSelected = true;
                        }

                    }
    }
}