﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
	public enum ImageState
	{
		NONE,
		PLAYING,
		PAUSED
	}

	public static ImageAnimation Instance;

	public List<Sprite> textureArray;

	public Image rendererDelegate;

	public bool useSharedMaterial = true;

	public bool doLoopAnimation = true;
	[SerializeField] private bool StartOnAwake;

	[HideInInspector]
	public ImageState currentAnimationState;

	private int indexOfTexture;

	private float idealFrameRate = 0.0416666679f;

	private float delayBetweenAnimation;

	public float AnimationSpeed = 5f;

	public float delayBetweenLoop;

	public Sprite originalSprite;

	public Transform parent;

	public int index=0;
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		originalSprite=rendererDelegate.sprite;
		parent=transform.parent;
		index=transform.GetSiblingIndex();

	}

	private void OnEnable()
	{
		if(StartOnAwake){
			StartAnimation();
		}
	}

	private void OnDisable()
	{
		//rendererDelegate.sprite = textureArray[0];
		StopAnimation();
	}

	private void AnimationProcess()
	{
		SetTextureOfIndex();
		indexOfTexture++;
		if (indexOfTexture == textureArray.Count)
		{
			indexOfTexture = 0;
			if (doLoopAnimation)
			{
				Invoke("AnimationProcess", delayBetweenAnimation + delayBetweenLoop);
			}
		}
		else
		{
			Invoke("AnimationProcess", delayBetweenAnimation);
		}
	}

	public void StartAnimation()
	{
		indexOfTexture = 0;
		if (currentAnimationState == ImageState.NONE)
		{
			RevertToInitialState();
			delayBetweenAnimation = idealFrameRate * (float)textureArray.Count / AnimationSpeed;
			currentAnimationState = ImageState.PLAYING;
			Invoke("AnimationProcess", delayBetweenAnimation);
		}
	}

	public void PauseAnimation()
	{
		if (currentAnimationState == ImageState.PLAYING)
		{
			CancelInvoke("AnimationProcess");
			currentAnimationState = ImageState.PAUSED;
		}
	}

	public void ResumeAnimation()
	{
		if (currentAnimationState == ImageState.PAUSED && !IsInvoking("AnimationProcess"))
		{
			Invoke("AnimationProcess", delayBetweenAnimation);
			currentAnimationState = ImageState.PLAYING;
		}
	}

	public void StopAnimation()
	{
		if (currentAnimationState != 0)
		{
			rendererDelegate.sprite = originalSprite;
			CancelInvoke("AnimationProcess");
			currentAnimationState = ImageState.NONE;
		}
	}

	public void ResetParenrt(){
		transform.SetParent(parent,true);
		transform.SetSiblingIndex(index);
	}
	public void RevertToInitialState()
	{
		indexOfTexture = 0;
		SetTextureOfIndex();
	}

	private void SetTextureOfIndex()
	{
		if (useSharedMaterial)
		{
			rendererDelegate.sprite = textureArray[indexOfTexture];
		}
		else
		{
			rendererDelegate.sprite = textureArray[indexOfTexture];
		}
	}
}
