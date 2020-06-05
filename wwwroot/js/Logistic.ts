import { Content } from "./ContentFunctions.js"

function ActivatePanel(element: HTMLElement): void
{
    PlanFact.classList.remove("active");
    Fact.classList.remove("active");

    element.classList.add("active");
}

async function ContentAdd(element: HTMLElement)
{
    Result.childNodes.forEach(s => s.remove());
    await Content.GetAllCities().then(t => Result.appendChild(t));

    document.querySelector("select")
        .addEventListener("change", function (e) { console.log(this.value); });
}

function Switch()
{
    ActivatePanel(this);
    ContentAdd(this);
}


const Result = document.querySelector("result");

const PlanFact = document.querySelector("#pFact");
const Fact = document.querySelector("#fact");

PlanFact.addEventListener("click", Switch);
Fact.addEventListener("click", Switch);




(async function main()
{
    ActivatePanel(PlanFact as HTMLElement);
    ContentAdd(PlanFact as HTMLElement);
})();