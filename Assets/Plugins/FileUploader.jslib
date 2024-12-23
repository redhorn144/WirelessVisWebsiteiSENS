var FileUploadingPlugin = {
 
    onPointerDown: function() {
      console.log("log 1");

      if (document.getElementById('FileUploadingPluginInput') == null)
            Init();

      if (document.getElementById('unity-canvas') == null)
        console.log("canvas is null");
      document.getElementById('unity-canvas').addEventListener('click', openFileDialog, false);

      function Init() {
          var inputFile = document.createElement('input');
          inputFile.setAttribute('type', 'file');
          inputFile.setAttribute('id', 'FileUploadingPluginInput');
       
          //filter certain files of type with following line:
          inputFile.setAttribute('accept', '.csv'); //or accept="audio/mp3"
       
          inputFile.style.visibility = 'hidden';
       
          inputFile.onclick = function (event) {
              console.log("log 3");
              this.value=null;
          };

          inputFile.onchange = function (evt) {
              //process file
              console.log("log 4");
              evt.stopPropagation();
              var fileInput = evt.target.files;
              if (!fileInput || !fileInput.length) {
                  return; // "no image selected"
              }

              var picURL = window.URL.createObjectURL(fileInput[0]);
              
              //do something with pic url
              SendMessage('Canvas', 'FileSelected', URL.createObjectURL(event.target.files[0]));
              SendMessage('Canvas', 'ChangeName', event.target.files[0].name);
          }
          document.body.appendChild(inputFile);
      }

      function openFileDialog() {
              console.log("log 2");
              document.getElementById('FileUploadingPluginInput').click();
              document.getElementById('unity-canvas').removeEventListener('click', openFileDialog);
      }

    }
};

mergeInto(LibraryManager.library, FileUploadingPlugin);