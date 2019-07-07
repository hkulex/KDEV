using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace alternatereality
{
    public class ScriptTemplateBuilderWindow : EditorWindow
    {
        private const string WINDOW_NAME = "Script Template Builder";
        private const float WINDOW_HEIGHT = 600f;
        private const float WINDOW_WIDTH = 400f;

        private Regex _titleFilter;
        private Regex _namespaceFilter;

        private string _title;
        private string _namespace;

        private bool _deriveFromMonobehaviour;

        private string[] _functionOptions;
        private string[] _saveOptions;

        private int _functionIndex;
        private int _saveIndex;


        [MenuItem("Tools/Script Template Builder")]
        public static void Open()
        {
            ScriptTemplateBuilderWindow window = GetWindow<ScriptTemplateBuilderWindow>();
            Texture icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Alternate Reality/Scripts/Tools/ScriptCustomizerWindow.png");
            GUIContent content = new GUIContent(WINDOW_NAME, icon);

            window.maxSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
            window.minSize = window.maxSize;
            window.titleContent = content;
        }


        /// <summary> 
        /// Initialize the script by setting variables.
        /// </summary> 
        private void Awake()
        {
            _titleFilter = new Regex("^[^/./\\:*?\"<>|]+$");
            _namespaceFilter = new Regex("[^a-zA-Z]");

            _title = "My Custom C# Script Template";
            _namespace = "company";

            _deriveFromMonobehaviour = true;

            _saveOptions = new string[] { "Unity Version (" + Application.unityVersion + ") (requires editor restart)", "Project (requires editor restart)" };
            _functionOptions = new string[] { "Awake"/*, "Start", "Update", "FixedUpdate", "LateUpdate", "OnGUI", "OnDisable", "OnEnable"*/ };

            _functionIndex = 0;
            _saveIndex = 0;
        }


        /// <summary> 
        /// Draws the window and its content.
        /// </summary> 
        private void OnGUI()
        {
            EditorGUIUtility.labelWidth = 180;
            EditorGUILayout.LabelField("The Script Template Builder allows you to create \r\nnew templates for C# scripts.", EditorStyles.boldLabel, GUILayout.Height(30));
            EditorGUILayout.Separator();

            _title = EditorGUILayout.TextField("Title:", _title);

            if (!_titleFilter.IsMatch(_title) || _title.Length == 0)
            {
                EditorGUILayout.HelpBox("The title contains invalid characters or none is specified.", MessageType.Warning);
            }
            
            _namespace = EditorGUILayout.TextField("Namespace:", _namespace);

            if (_namespaceFilter.IsMatch(_namespace) || _namespace.Length == 0)
            {
                EditorGUILayout.HelpBox("The namespace contains invalid characters or none is specified.", MessageType.Warning);
            }

            _deriveFromMonobehaviour = EditorGUILayout.Toggle("Derive from MonoBehaviour:", _deriveFromMonobehaviour);
            
            if (_deriveFromMonobehaviour)
                _functionIndex = EditorGUILayout.MaskField("Add default functions: ", _functionIndex, _functionOptions);

            _saveIndex = EditorGUILayout.Popup("Save template to: ", _saveIndex, _saveOptions);
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Template preview:");
            EditorGUILayout.LabelField(Build(), EditorStyles.textField, GUILayout.Height(300));
            GUILayout.FlexibleSpace();

            if (_titleFilter.IsMatch(_title) && _title.Length > 0 && !_namespaceFilter.IsMatch(_namespace) && _namespace.Length > 0)
                if (GUILayout.Button("Create and save template"))
                    Save();

            Repaint();
        }

        
        /// <summary> 
        /// Build the script template based on input.
        /// </summary> 
        private string Build()
        {
            string script = "";

            script += "using UnityEngine;\r\n\r\n";
            script += "namespace " + _namespace + "\r\n{\r\n";
            script += "    public class #SCRIPTNAME# ";

            if (_deriveFromMonobehaviour)
                script += ": MonoBehaviour";

            script += "\r\n    {\r\n        ";

            if (_functionIndex != 0)
                script += "private void Awake()\r\n        {\r\n        \r\n        }";

            script += "\r\n    }\r\n";
            script += "}\r\n";

            return script;
        }


        /// <summary> 
        /// Save the script template to a .txt file based on the Unity Editor/project directory.
        /// </summary> 
        private void Save()
        {
            string[] files = GetScriptTemplatesFrom(GetDirectory(_saveIndex));
            int index = 100;
            
            Array.Sort(files); // Sort the array alphabetically because order is not guaranteed

            foreach (string file in files)
            {
                int.TryParse(file.Substring(0, 2), out index);

                if (index >= 0)
                    break;
            }

            string fileName = (index == -1 ? 99 : index - 1) + "-" + _title + "-New" + (_deriveFromMonobehaviour ? "MonoBehaviour" : "") + "Script.cs.txt";

            if (!Directory.Exists(GetDirectory(_saveIndex)))
                Directory.CreateDirectory(GetDirectory(_saveIndex));

            StreamWriter writer = new StreamWriter(GetDirectory(_saveIndex) + fileName);

            writer.Write(Build());
            writer.Close();

            AssetDatabase.Refresh();
        }
        

        /// <summary> 
        /// Get the directory relative to the current project/Unity version.
        /// </summary> 
        /// <param index="Path index"></param> 
        private string GetDirectory(int index)
        {
            switch (index)
            {
                case 0:
                    return EditorApplication.applicationContentsPath + "/Resources/ScriptTemplates/";

                case 1:
                    return Application.dataPath + "/" + _namespace + "/ScriptTemplates/";
            }

            return "";
        }


        /// <summary> 
        /// Load a list of script templates already in the specified folder.
        /// </summary> 
        /// <param directory="Target directory"></param> 
        private string[] GetScriptTemplatesFrom(string directory)
        {
            List<string> files = new List<string>();

            if (Directory.Exists(directory))
                files.AddRange(Directory.GetFiles(directory).Select(file => file.Substring(directory.Length)));

            return files.ToArray();
        }
    }
}