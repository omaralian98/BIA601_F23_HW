import axios from "axios";
import { useEffect, useState } from "react";
import useFormData from "../../store";

export const useGet = (url) => {
  const [dataGet, setDataGet] = useState({});

  useEffect(() => {
    axios
      .get(url)
      .then((req) => setDataGet(req.data))
      .catch((error) => {
        console.log(error.message);
      });
  }, []);
  return { dataGet };
};

export const usePost = () => {
  const { setResponse } = useFormData();
  const [dataPost, setDataPost] = useState(null);

  const handleSubmit = (url, postData) => {
    axios
      .post(url, postData)
      .then((response) => {
        if (response.data) {
          setResponse(response.data);
        }
      })
      .catch((error) => {
        console.error("Error posting data:", error.message);
      });
  };

  return { dataPost, handleSubmit };
};
