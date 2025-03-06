"use strict";

/* -------------------------------------------------------------------------- */

/*                              Config                                        */

/* -------------------------------------------------------------------------- */
var CONFIG = {
  isNavbarVerticalCollapsed: false,
  theme: 'dark',
  isRTL: false,
  isFluid: true,
  navbarStyle: 'transparent',
  navbarPosition: 'vertical'
};
Object.keys(CONFIG).forEach(function (key) {
  if (localStorage.getItem(key) === null) {
    localStorage.setItem(key, CONFIG[key]);
  }
});

if (JSON.parse(localStorage.getItem('isNavbarVerticalCollapsed'))) {
  document.documentElement.classList.add('navbar-vertical-collapsed');
}

if (localStorage.getItem('theme') === 'dark') {
    document.documentElement.classList.add('dark');
    updateDevExtremeTheme(localStorage.getItem('theme'));
}
// DevExtreme Temas�n� G�ncelle
function updateDevExtremeTheme(theme) {
    var themeLink = document.getElementById("devextreme-theme");
    if (themeLink) {
        var newTheme = theme === 'dark' ? 'dark' : 'light';
        themeLink.href = `/devextreme/css/dx.volleytics.${newTheme}.css`;
    }
}
// Sayfa y�klendi�inde do�ru DevExtreme temas�n� y�kle
updateDevExtremeTheme(localStorage.getItem('theme'));