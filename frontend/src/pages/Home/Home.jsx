import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import useFormData from "../../store";
import { usePost } from "../../Tools/APIs/index";
import { generateRandomData } from "../../Tools/help_functions/genrateRandom";
import SectionHome from "../../components/SectionHome";
import { generateLocationNames } from "../../Tools/help_functions/generateLocationNames";
import random1 from "../../randomData/1.json";
import random2 from "../../randomData/2.json";
import random3 from "../../randomData/3.json";
import random4 from "../../randomData/4.json";
import random5 from "../../randomData/5.json";
import random6 from "../../randomData/6.json";
import random7 from "../../randomData/7.json";
import random8 from "../../randomData/8.json";
import random9 from "../../randomData/9.json";
import random10 from "../../randomData/10.json";
import random11 from "../../randomData/11.json";
import random12 from "../../randomData/12.json";
import random13 from "../../randomData/13.json";
import { getRandomInt } from "../../Tools/help_functions/genrateRandom";

const randomData = [
  random1,
  random2,
  random3,
  random4,
  random5,
  random6,
  random7,
  random8,
  random9,
  random10,
  random11,
  random12,
  random13,
];

const modesDesc = [
  {
    title: "Mode 1",
    desc: "In this mode, the application identifies the optimal set of items that fit into a knapsack and determines the best route between multiple cities. This involves solving the 0/1 Knapsack problem and the TSP independently.",
  },
  {
    title: "Mode 2",
    desc: "Mode 2 builds on Mode 1 by associating each item with a specific city. The objective is to select items that fit into the knapsack and deliver them to their respective locations, without the necessity of visiting all cities.",
  },
  {
    title: "Mode 3",
    desc: "This mode introduces multiple salesmen, each with their own knapsack, distributing the items among them.",
  },
  {
    title: "Mode 4",
    desc: "Here, warehouses are introduced, with each vehicle having a distinct starting and ending point.",
  },
  {
    title: "Mode 5",
    desc: "In this mode, items are linked to specific warehouses, meaning each item has a designated pick-up and drop-off location.",
  },
  {
    title: "Mode 6",
    desc: "This mode incorporates a time penalty for items, which is proportional to their weight and value. This represents the real-world scenario where picking up and dropping off items requires time.",
  },
];

const Home = () => {
  const navigate = useNavigate();
  const { handleSubmit, successfulPost } = usePost();
  const [isGeneratedRandom, setIsGeneratedRandom] = useState(false);
  const [preDefinedCases, setPreDefinedCases] = useState(false);

  const {
    setFormDataToSend,
    formDataToSend,
    mode,
    addLocations,
    formDataStorage,
    addLoctaionsTime,
    changeMode,
  } = useFormData();

  useEffect(() => {
    setIsGeneratedRandom(false);
    setFormDataToSend(null);
  }, [mode.link]);

  const handleClick = () => {
    handleSubmit(mode.link, formDataToSend);
  };

  const distances = [];

  useEffect(() => {
    for (
      let i = 0;
      i < formDataStorage?.locations?.locations_name?.length;
      i++
    ) {
      let temp = [];
      for (
        let j = 0;
        j < formDataStorage?.locations?.locations_name?.length;
        j++
      ) {
        if (
          formDataStorage?.locations?.locations_name[i] ===
          formDataStorage?.locations?.locations_name[j]
        )
          continue;
        temp.push({
          start: formDataStorage?.locations?.locations_name[i],
          end: formDataStorage?.locations?.locations_name[j],
          distance: formDataToSend?.distances[i][j],
        });
      }
      distances.push(temp);
      temp = [];
    }
    addLoctaionsTime(distances);
  }, [formDataStorage?.locations?.locations_name, formDataToSend?.distances]);

  useEffect(() => {
    if (successfulPost) {
      navigate("map/nodes");
      setIsGeneratedRandom(false);
    }
  }, [successfulPost]);

  useEffect(() => {
    addLocations(generateLocationNames(formDataToSend?.distances?.length));
  }, [formDataToSend?.distances]);

  return (
    <div className="flex flex-col gap-14 h-full w-full">
      <div className="text-center flex flex-col gap-4">
        <h1 className="text-7xl max-sm:text-5xl font-semibold text-yellow-500">
          405 Found
        </h1>
        <p className="text-xl font-semibold max-sm:text-lg text-gray-800">
          We find the best way for you!
        </p>
      </div>
      <div className="mx-auto text-center w-2/3 space-y-6">
        <p className="text-gray-700 bg-gray-300 rounded-xl p-10 text-lg">
          This project addresses several optimization challenges, including
          various scenarios involving the knapsack problem and the traveling
          salesman problem (TSP). The backend is developed using ASP.NET, while
          the frontend is built with React.
        </p>
      </div>
      <div className="flex gap-10 justify-evenly max-md:flex-col items-center">
        <div className="flex gap-4 flex-col justify-center items-center">
          <button
            onClick={() => {
              setFormDataToSend(randomData[getRandomInt(0, 14)]);
              changeMode("mode6");
              setPreDefinedCases(true);
              setIsGeneratedRandom(false);
            }}
            className="w-fit h-fit max-sm:max-auto text-white font-semibold p-4 bg-blue-500 hover:bg-blue-400 text-center rounded-xl">
            Try it with pre-defined cases
          </button>
          {preDefinedCases && (
            <button
              onClick={handleClick}
              className="w-fit h-fit max-sm:max-auto text-white font-semibold p-4 bg-blue-500 hover:bg-blue-400 text-center rounded-xl">
              See results on the map
            </button>
          )}
        </div>
        <h1 className="text-4xl font-semibold">OR</h1>

        <div className="flex gap-4 flex-col justify-center items-center">
          <button
            onClick={() => {
              setFormDataToSend(generateRandomData(mode.link));
              setIsGeneratedRandom(true);
              setPreDefinedCases(false);
            }}
            className="w-fit h-fit max-sm:max-auto text-white font-semibold p-4 bg-black hover:opacity-80 text-center rounded-xl">
            Try it with random cases
          </button>
          {isGeneratedRandom && (
            <button
              onClick={handleClick}
              className="w-fit h-fit max-sm:max-auto text-white font-semibold p-4 bg-black hover:opacity-80 text-center rounded-xl">
              See results on the map
            </button>
          )}
        </div>
        <h1 className="text-4xl font-semibold">OR</h1>
        <Link
          to="initial"
          className="w-fit h-fit max-sm:max-auto text-gray-800 font-semibold p-4 bg-yellow-400 hover:bg-yellow-500 text-center rounded-xl">
          Try it with real data
        </Link>
      </div>
      <h1 className="text-gray-700 font-bold text-5xl mx-auto">Modes</h1>
      <div className="flex gap-4 my-5 flex-wrap">
        {modesDesc.map((e) => (
          <SectionHome title={e.title} desc={e.desc} key={e.title} />
        ))}
      </div>
      <div className="bg-gray-300 rounded-xl p-4 flex flex-col gap-4 justify-between mx-auto max-sm:w-[90%]">
        <h2 className="text-2xl font-bold">Features</h2>
        <ul className="list-disc list-inside">
          <li className="text-lg">
            Variation of programic languages (javascript, python, C#) and more
          </li>
          <li className="text-lg">Use several algorithms</li>
          <li className="text-lg">Comparison between two algorithms</li>
          <li className="text-lg">Comparison between two languages</li>
        </ul>
        <Link
          to="benchmark"
          className="bg-blue-500 text-white py-2 px-7 text-lg font-semibold rounded-lg text-center hover:bg-blue-400 w-fit self-end">
          Try it
        </Link>
      </div>
    </div>
  );
};

export default Home;
