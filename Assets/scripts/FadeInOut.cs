/**
*    Title:"":项目
*          主题 :
*    Description:
*          功能：场景的淡入淡出的效果
*    Date:2017.10.12
*    Version:Unity5.5.4
*    Modify Recoder:
*    Operator:
*
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


    public class FadeInOut : MonoBehaviour
    {

        #region 单例模式

        //设置静态的字段来模拟全局的变量(开始为空)
        private static FadeInOut _instance = null;
        /// <summary>
        /// 只读属性的设置
        /// </summary>
        public static FadeInOut Instance
        {
            get
            {
                //如果一开始的为空的话
                if (_instance == null)
                {
                    //进行类的实例化的操作
                    _instance = new FadeInOut();
                }
                //下一个访问的话_instance就不为空直接返回字段
                return _instance;
            }
        }
        private FadeInOut() { }

        #endregion

        #region 字段和属性的定义

        //渐变的速率
        public float floatColorChangeSpeed = 1f;
        //RawImage对象
        public GameObject goRawImage;
        //RawImage组件
        private RawImage _rawImage;
        //屏幕是否要逐渐清晰(默认是需要的)
        private bool _isSceneToClear = true;
        //屏幕是否需要逐渐变暗(默认是不需要的)
        private bool _isSceneToBlack = false;

        #endregion

        void Awake()
        {
            //如果goRawImage不为空的话
            if (goRawImage)
            {
                //得到RawImage组件
                _rawImage = goRawImage.GetComponent<RawImage>();
            }
        }
        void Update()
        {
            if (sceneStarting)
            {
                StartScene();
            }
        }
        private bool sceneStarting = true;
        #region 公共方法的定义

        /// <summary>
        /// 设置场景的淡入
        /// </summary>
        public void SetSceneToClear()
        {
            _isSceneToClear = true;
            _isSceneToBlack = false;
        }

        /// <summary>
        /// 设置场景的淡出
        /// </summary>
        public void SetSceneToBlack()
        {
            _isSceneToClear = false;
            _isSceneToBlack = true;
        }

        #endregion

        #region 私有方法的定义

        /// <summary>
        /// 屏幕逐渐清晰(淡入)
        /// </summary>
        private void FadeToClear()
        {
            //插值运算
            _rawImage.color = Color.Lerp(_rawImage.color, Color.clear, floatColorChangeSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 屏幕逐渐暗淡(淡出)
        /// </summary>
        private void FadeToBlack()
        {
            //插值运算
            _rawImage.color = Color.Lerp(_rawImage.color, Color.black, floatColorChangeSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 屏幕的淡入
        /// </summary>
        private void StartScene()
        {
            FadeToClear();
            //当我们的a值小于等于0.05f的时候 就相当于完全透明了
            if (_rawImage.color.a <= 0.05f)
            {
                //设置为完全透明
                _rawImage.color = Color.clear;
                //组件的开关设置为关闭的状态
                _rawImage.enabled = false;
                //布尔条件设置为false
                _isSceneToClear = false;
                _isSceneToBlack = false;
                sceneStarting = false;
            }
        }

        /// <summary>
        /// 屏幕的淡出
        /// </summary>
        /// 
        private void Start()
        {
            _rawImage = this.GetComponent<RawImage>();
           

        }
        public void EndScene()
        {
            //组件的打开
            _rawImage.enabled = true;
            FadeToBlack();
            //当前的阿尔法值大于0.95f的时候 表示已经接近于完全不透明的状态
            if (_rawImage.color.a >= 0.95f)
            {
                //设置为完全不透明的状态 
                _rawImage.color = Color.black;
                //布尔条件当到达指定的阿尔法值得时候设置为false
                _isSceneToBlack = false;
            sceneStarting = true;
                SceneManager.LoadScene("Demo");
            }
        }

        #endregion

    }//class_end
