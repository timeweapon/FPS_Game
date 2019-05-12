using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationExtensions
{
    public static IEnumerator PPlay(this Animation animation, string clipName)
    {
        bool useTimeScale = false;
        //Debug.Log(&quot; Overwritten Play animation, useTimeScale ? &quot; +useTimeScale);
        //We Don't want to use timeScale, so we have to animate by frame..
        if (!useTimeScale)
        {
            //Debug.Log(&quot; Started this animation!(&quot; +clipName + &quot; ) &quot;);
            AnimationState _currState = animation[clipName];
            bool isPlaying = true;
            float _startTime = 0F;
            float _progressTime = 0F;
            float _timeAtLastFrame = 0F;
            float _timeAtCurrentFrame = 0F;
            float deltaTime = 0F;


            animation.Play(clipName);

            _timeAtLastFrame = Time.realtimeSinceStartup;
            while (isPlaying)
            {
                _timeAtCurrentFrame = Time.realtimeSinceStartup;
                deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
                _timeAtLastFrame = _timeAtCurrentFrame;

                _progressTime += deltaTime;
                _currState.normalizedTime = _progressTime / _currState.length;
                animation.Sample();

                //Debug.Log(_progressTime);

                if (_progressTime >= _currState.length)
                {
                    //Debug.Log(&quot;Bam! Done animating&quot;);
                    if (_currState.wrapMode != WrapMode.Loop)
                    {
                        //Debug.Log(&quot;Animation is not a loop anim, kill it.&quot;);
                        //_currState.enabled = false;
                        isPlaying = false;
                    }
                    else
                    {
                        //Debug.Log(&quot;Loop anim, continue.&quot;);
                        _progressTime = 0.0f;
                    }
                }

                yield return new WaitForEndOfFrame();
            }
            yield return null;
            //if (onComplete != null)
            //{
            //    //Debug.Log(&quot; Start onComplete&quot;);
            //    onComplete();
            //}
        }
        else
        {
            animation.Play(clipName);
        }
    }
}
