using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using TMPro;
using System.IO;
using Random = System.Random;
using System.Diagnostics;
using IPA.Utilities;

namespace MemeSaber.UI
{
    class MemeViewController : IInitializable, IDisposable
    {
        string memesFolderPath = Path.Combine(UnityGame.InstallPath, @"UserData\MemeSaber\DefaultMemes\");
        string customMemesFolderPath = Path.Combine(UnityGame.InstallPath, @"UserData\MemeSaber\CustomMemes\");

        List<Sprite> memeList = new List<Sprite>();
        List<int> usedMemes = new List<int>();

        string[] extensionsArray = {
            ".png",
            ".jpg",
            ".jpeg" };

        Random random = new Random();

        private ResultsViewController resultsViewController;

        [UIComponent("meme-image")]
        private ImageView memeImage;

        [UIComponent("root")]
        private RectTransform rootTransform;

        [UIComponent("all-meme-used-text")]
        private TextMeshProUGUI modifiedText;

        MemeViewController(ResultsViewController resultsViewController)
        {
            this.resultsViewController = resultsViewController;
        }

        public void Initialize()
        {
            BSMLParser.instance.Parse(BeatSaberMarkupLanguage.Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "MemeSaber.UI.MemeView.bsml"), resultsViewController.gameObject, this);
            rootTransform.Translate(new Vector3(0, 0.90f, 0));
            resultsViewController.didActivateEvent += ResultsViewController_didActivateEvent;

            Plugin.Log.Info("Memes folder path: " + memesFolderPath);
            Plugin.Log.Info("Custom Memes folder path " + customMemesFolderPath);

            DirectoryInfo memesDirectory = new DirectoryInfo(memesFolderPath);
            DirectoryInfo customMemesDirectory = new DirectoryInfo(customMemesFolderPath);

            FileInfo[] memeFiles = memesDirectory.GetFiles();
            FileInfo[] customMemesFiles = customMemesDirectory.GetFiles();



            foreach (FileInfo imagePath in memeFiles)
            {

                Plugin.Log.Info("Image path: " + imagePath.ToString());

                if (extensionsArray.Contains(Path.GetExtension(imagePath.ToString()).ToLower()) == true)
                {
                    try
                    {
                        byte[] byteImageArray = File.ReadAllBytes(imagePath.ToString());
                        Sprite imageSprite = BeatSaberMarkupLanguage.Utilities.LoadSpriteRaw(byteImageArray);

                        if (imageSprite != null)
                        {
                            memeList.Add(imageSprite);
                        }
                    }

                    catch (Exception error)
                    {
                        Plugin.Log.Info("Error creating sprite image: " + error.ToString());
                    }
                }
            }
            
            if (customMemesFiles.Length > 0)
            {
                foreach (FileInfo imagePath in customMemesFiles)
                {
                    Plugin.Log.Info("Custom image path: " + imagePath.ToString());

                    if (extensionsArray.Contains(Path.GetExtension(imagePath.ToString()).ToLower()) == true)
                    {
                        try
                        {
                            byte[] byteImageArray = File.ReadAllBytes(imagePath.ToString());
                            Sprite imageSprite = BeatSaberMarkupLanguage.Utilities.LoadSpriteRaw(byteImageArray);

                            if (imageSprite != null)
                            {
                                memeList.Add(imageSprite);
                            }
                        }

                        catch (Exception error)
                        {
                            Plugin.Log.Info("Error creating sprite image: " + error.ToString());
                        }
                    }
                    else
                    {
                        Plugin.Log.Info("Wrong extension for file : " + imagePath.ToString());
                    }
                }

            }
        }

        private void ResultsViewController_didActivateEvent(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            modifiedText.text = "";
            usedMemes.Clear();
            OnButtonClick();
        }

        public void Dispose()
        {
            resultsViewController.didActivateEvent -= ResultsViewController_didActivateEvent;
        }

       
        [UIAction("on-click-button")]
        public void OnButtonClick()
        {
            while (true)
            {

                int randomNumber = random.Next(0, memeList.Count());

                if (usedMemes.Contains(randomNumber) == false)
                {
                    
                    memeImage.sprite = memeList[randomNumber];
                    usedMemes.Add(randomNumber);

                    break;
                }

                else if (usedMemes.Count() == memeList.Count())
                {
                    usedMemes.Clear();

                    modifiedText.text = "No more new memes!";
                }

            }          
        }
    }
}
