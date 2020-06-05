﻿export namespace Content
{
    export async function GetAllCities()
    {
        let result = document.createElement("div");
        result.setAttribute("id", "city-list")

        let request = await fetch("http://localhost:3189/LogisticAJAX/GetCities",
            {
                method: "post"
            });
        let data = await request.text();

        result.innerHTML = data;

        return result;
    }
}