import React from "react";
import NewsTicker from "../components/NewsTicker";

const Footer = () => {
  return (
    <footer className="bg-gray-800 rounded-xl flex items-center justify-center h-[10vh]">
      <div className="flex justify-center items-center p-5 w-full">
        <NewsTicker
          newsItems={[
            "omar_192751",
            "mohannad_192707",
            "ghena_114233",
            "Farah_194030",
          ]}
        />
      </div>
    </footer>
  );
};

export default Footer;
