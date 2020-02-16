let LeftPanel = document.querySelector(".left-panel");
let RigthPanel = document.querySelector(".rigth-panel");
let Result = document.querySelector("result");
//let Content = document.createDocumentFragment();
//let Data = document.createElement("div");
//Data.innerText = "Some div";
//Data.classList.add("some-class");
//Content.appendChild(Data);
LeftPanel.addEventListener("click", (e) => {
    LeftPanel.classList.add("active");
    RigthPanel.classList.remove("active");
    Result.textContent = "Left";
    console.log(this);
});
RigthPanel.addEventListener("click", (e) => {
    RigthPanel.classList.add("active");
    LeftPanel.classList.remove("active");
    Result.textContent = "Rigth";
    console.log(this);
});
(function main() {
    LeftPanel.classList.add("active");
    Result.textContent = "Left";
})();
//# sourceMappingURL=Logistic.js.map