mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Loaded File!");
  },

  FreeNotification: function () {
    window.alert("Use w, a, s, and d to move the cammera. Click and drag to change orientations.");
  },

  FixedNotification: function () {
    window.alert("Press space to change camera location.");
  },

});