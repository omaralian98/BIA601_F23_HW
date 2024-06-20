import React from "react";
import { Link } from "react-router-dom";
import DropDown from "../components/DropDown";
import logo from "../assets/Group 1 (2).svg";

const NavBar = () => {
  return (
    <nav className="bg-gray-800 rounded-xl flex items-center p-4 justify-around h-[12vh]">
      <Link to="/" className="cursor-pointer rounded-full w-32 max-sm:w-24">
        <img src={logo} alt="" className="object-cover rounded-xl" />
      </Link>
      <DropDown />
      <Link
        to="settings"
        className="text-xl px-4 py-2 bg-yellow-500 font-semibold rounded-xl text-white">
        Settings
      </Link>
    </nav>
  );
};

export default NavBar;
