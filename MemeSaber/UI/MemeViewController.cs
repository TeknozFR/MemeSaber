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
        string memesFolderPath = Path.Combine(UnityGame.InstallPath, @"UserData\MemeSaber\DefaultMemes");

        List<Sprite> memeList = new List<Sprite>();
        List<int> usedMemes = new List<int>();

        Random random = new Random();

        private ResultsViewController resultsViewController;

        [UIComponent("meme -image")]
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
            rootTransform.Translate(new Vector3(0, 0.80f, 0));
            resultsViewController.didActivateEvent += ResultsViewController_didActivateEvent;

            DirectoryInfo memesDirectory = new DirectoryInfo(memesFolderPath);
            FileInfo[] Files = memesDirectory.GetFiles();
            foreach (FileInfo imagePath in Files)
            {
                string imageFileName = imagePath.ToString().Replace(memesFolderPath, "");
                Plugin.Log.Info("Loaded meme file: " + imageFileName);
                memeList.Add(BeatSaberMarkupLanguage.Utilities.FindSpriteInAssembly(("MemeSaber.Memes." + imageFileName)));
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
