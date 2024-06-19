import React from "react";

const SectionHome = ({ title, desc }) => {
  return (
    <div className="w-[30%] max-md:w-2/5 max-sm:w-[90%] mx-auto bg-gray-700 rounded-2xl p-4">
      <div className={`flex flex-col text-center gap-4`}>
        <h1 className="text-yellow-400 text-3xl font-bold">{title}</h1>
        <p className="text-gray-300 font-semibold text-lg">{desc}</p>
      </div>
    </div>
  );
};

export default SectionHome;
