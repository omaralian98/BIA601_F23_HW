import React, { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import useFormData from "../store";

const DropDown = () => {
  const { changeMode, mode, setFormDataStorage } = useFormData();
  const navigate = useNavigate();

  const [show, setShow] = useState(false);
  const modes = [
    { name: "Mode 1", link: "mode1" },
    { name: "Mode 2", link: "mode2" },
    { name: "Mode 3", link: "mode3" },
    { name: "Mode 4", link: "mode4" },
  ];

  return (
    <div className="relative">
      <button
        onClick={() => setShow(!show)}
        className="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center inline-flex items-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
        {mode.name}
        <svg
          className="w-2.5 h-2.5 ms-3"
          aria-hidden="true"
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 10 6">
          <path
            stroke="currentColor"
            strokeLinecap="round"
            strokeLinejoin="round"
            strokeWidth="2"
            d="m1 1 4 4 4-4"
          />
        </svg>
      </button>
      {show && (
        <div className="z-10 absolute top-[110%] -left-1/3 bg-white divide-y divide-gray-100 rounded-lg shadow w-44 dark:bg-gray-700">
          <ul
            className="py-2 text-sm text-gray-700 dark:text-gray-200"
            aria-labelledby="dropdownDefaultButton">
            {modes.map((mode) => (
              <li
                onClick={() => {
                  changeMode(mode);
                  navigate("/");
                  setFormDataStorage({
                    capacity: 0,
                    objects_count: 0,
                    locations_count: 0,
                    trucks_count: 0,
                  });
                }}
                className="w-full">
                <button className="block w-full text-center px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">
                  {mode.name}
                </button>
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default DropDown;
