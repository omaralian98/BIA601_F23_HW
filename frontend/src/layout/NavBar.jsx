import React from "react";
import { Link } from "react-router-dom";
import DropDown from "../components/DropDown";

const NavBar = () => {
  return (
    <nav className="h-[10vh] bg-gray-800 rounded-xl flex items-center p-4 justify-around">
      <Link to="/" className="cursor-pointer">
        <img src="" alt="" width={60} height={60} />
      </Link>
      <DropDown />
    </nav>
  );
};

export default NavBar;
