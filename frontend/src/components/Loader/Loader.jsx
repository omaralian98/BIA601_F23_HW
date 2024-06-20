import React from "react";
import { FaTruckFast } from "react-icons/fa6";
import "./Loader.css";

const Loader = () => {
  return (
    <div className="absolute w-screen h-screen">
      <div className="fixed bg-black/60 w-full h-full z-10"></div>
      <div className="flex justify-center items-center w-full h-full">
        <FaTruckFast size={80} className="z-20 truck-animation" color="white" />
      </div>
    </div>
  );
};

export default Loader;
