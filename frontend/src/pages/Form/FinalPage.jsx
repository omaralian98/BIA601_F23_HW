import React from "react";
import useFormData from "../../store";
import { usePost } from "../../Tools/APIs";
import { useNavigate } from "react-router-dom";

const FinalPage = () => {
  const navigate = useNavigate();
  const { handleSubmit } = usePost();
  const { formDataToSend } = useFormData();

  return (
    <div className="flex flex-col justify-center items-center gap-5">
      <h1 className="text-xl text-gray-800 font-bold">
        you're finally finished
      </h1>
      <button
        onClick={() => {
          handleSubmit(
            `https://7203-5-155-15-53.ngrok-free.app/api/mode2`,
            formDataToSend
          );
          navigate("/");
        }}
        className="bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-gray-300">
        Submit
      </button>
    </div>
  );
};

export default FinalPage;
