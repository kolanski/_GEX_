﻿var s = [
"6041413497927745769261708",
"1743505804167185577090041",
"6399711285183153201919902",
"5070209091923866972288008",
"4112412542395462566601478",
"7247417183423459489021265",
"2103857281096562558702801",
    ]
i    = 0;
window.setInterval(function () {
    document.getElementById("key").value = s[i]; i++; document.getElementById("add-game-submit").click();
}, 5000);