

let LeftPanel = document.querySelector(".left-panel");
let RigthPanel = document.querySelector(".rigth-panel");
let Result = document.querySelector("result");


LeftPanel.addEventListener("click", (e) =>
{
    LeftPanel.classList.add("active");
    RigthPanel.classList.remove("active");
    Result.textContent = "Left";
    console.log(this);

});

RigthPanel.addEventListener("click", (e) =>
{
    RigthPanel.classList.add("active");
    LeftPanel.classList.remove("active");
    Result.textContent = "Rigth";
    console.log(this);
});

(function main(): void
{
    LeftPanel.classList.add("active");
    Result.textContent = "Left";
})();