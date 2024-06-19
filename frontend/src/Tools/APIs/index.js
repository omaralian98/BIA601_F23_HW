import axios from "axios";
import { useEffect, useState } from "react";
import useFormData from "../../store";
import base64 from "base-64";

const username = "11182471";
const password = "60-dayfreetrial";

// Encode the credentials in Base64 using base-64 library
const token = base64.encode(`${username}:${password}`);

export const useGet = (url) => {
  const [dataGet, setDataGet] = useState({});

  useEffect(() => {
    axios
      .get(url, {
        headers: {
          Authorization: `Basic ${token}`,
        },
      })
      .then((req) => setDataGet(req.data))
      .catch((error) => {
        console.log(error.message);
      });
  }, [url]);
  return { dataGet };
};

export const usePost = () => {
  const { setResponse } = useFormData();
  const [successfulPost, setSuccessfulPost] = useState(false);

  const handleSubmit = (url, postData) => {
    setSuccessfulPost(false);
    axios
      .post(url, postData, {
        headers: {
          Authorization: `Basic ${token}`,
          "Content-Type": "application/json",
          "Access-Control-Allow-Origin": "*",
          "Access-Control-Allow-Headers": "*",
          "Control-Allow-Credentials": true,
        },
      })
      .then((response) => {
        if (response.data) {
          setResponse(response.data);
          setSuccessfulPost(true);
        }
      })
      .catch((error) => {
        console.error("Error posting data:", error.message);
      });
  };

  return { handleSubmit, successfulPost };
};
