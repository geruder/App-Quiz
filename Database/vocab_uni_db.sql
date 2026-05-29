-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 25, 2025 at 07:11 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `vocab_uni_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `words`
--

DROP TABLE IF EXISTS `words`;
CREATE TABLE `words` (
  `word_id` int(11) NOT NULL,
  `category_id` int(11) DEFAULT NULL,
  `root_word_id` int(11) DEFAULT NULL,
  `english_word` varchar(100) NOT NULL,
  `turkish_meaning` varchar(255) NOT NULL,
  `cefr_level` enum('A1','A2','B1','B2','C1','C2') NOT NULL,
  `created_by` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `words`
--

INSERT INTO `words` (`word_id`, `category_id`, `root_word_id`, `english_word`, `turkish_meaning`, `cefr_level`, `created_by`) VALUES
(1, 1, NULL, 'Apple', 'Elma', 'A1', NULL),
(2, 1, NULL, 'Run', 'Koşmak', 'A1', NULL),
(3, 2, NULL, 'Negotiate', 'Müzakere etmek', 'B2', NULL),
(4, 3, NULL, 'Surgery', 'Ameliyat', 'C1', NULL),
(6, 1, NULL, 'Freedom', 'Hürriyet', 'C1', 2),
(8, 1, NULL, 'Computer', 'Bilgisayar', 'B1', 2),
(9, 1, NULL, 'Book', 'Kitap', 'A1', 2),
(10, 1, NULL, 'Water', 'Su', 'A1', 2),
(11, 1, NULL, 'Wisdom', 'Bilgelik', 'B2', 2),
(12, 1, NULL, 'Presentation', 'Sunum', 'B1', 8),
(13, 1, NULL, 'Book', 'Kitap', 'A1', NULL),
(14, 1, NULL, 'Water', 'Su', 'A1', NULL),
(15, 1, NULL, 'House', 'Ev', 'A1', NULL),
(16, 1, NULL, 'School', 'Okul', 'A1', NULL),
(17, 1, NULL, 'Friend', 'Arkadaş', 'A1', NULL),
(18, 1, NULL, 'Weather', 'Hava Durumu', 'A2', NULL),
(19, 1, NULL, 'Decide', 'Karar Vermek', 'A2', NULL),
(20, 1, NULL, 'Dangerous', 'Tehlikeli', 'A2', NULL),
(21, 3, NULL, 'Nurse', 'Hemşire', 'A2', NULL),
(22, 3, NULL, 'Headache', 'Baş Ağrısı', 'A2', NULL),
(23, 1, NULL, 'Environment', 'Çevre', 'B1', NULL),
(24, 1, NULL, 'Government', 'Hükümet', 'B1', NULL),
(25, 2, NULL, 'Employee', 'Çalışan', 'B1', NULL),
(26, 2, NULL, 'Salary', 'Maaş', 'B1', NULL),
(27, 3, NULL, 'Treatment', 'Tedavi', 'B1', NULL),
(28, 2, NULL, 'Investment', 'Yatırım', 'B2', NULL),
(29, 2, NULL, 'Strategy', 'Strateji', 'B2', NULL),
(30, 2, NULL, 'Negotiation', 'Müzakere', 'B2', NULL),
(31, 1, NULL, 'Significant', 'Önemli', 'B2', NULL),
(32, 1, NULL, 'Appropriate', 'Uygun', 'B2', NULL),
(33, 1, NULL, 'Comprehensive', 'Kapsamlı', 'C1', NULL),
(34, 1, NULL, 'Subsequent', 'Sonraki', 'C1', NULL),
(35, 3, NULL, 'Diagnosis', 'Teşhis', 'C1', NULL),
(36, 2, NULL, 'Revenue', 'Gelir / Hasılat', 'C1', NULL),
(37, 1, NULL, 'Inevitably', 'Kaçınılmaz olarak', 'C1', NULL),
(38, 1, NULL, 'Profound', 'Derin / Kapsamlı', 'C2', NULL),
(39, 1, NULL, 'Ambiguous', 'Muğlak / Belirsiz', 'C2', NULL),
(40, 1, NULL, 'Ephemeral', 'Geçici / Kısa süreli', 'C2', NULL),
(41, 2, NULL, 'Acquisition', 'Edinim / Devralma', 'C2', NULL),
(42, 3, NULL, 'Resilience', 'Direnç / Dayanıklılık', 'C2', NULL),
(43, 1, NULL, 'Serendipity', 'Mutlu Tesadüf', 'C2', NULL),
(44, 1, NULL, 'Meticulous', 'Titiz / İtinalı', 'C2', NULL),
(45, 1, NULL, 'Ambition', 'Hırs / Tutku', 'B2', NULL),
(46, 1, NULL, 'Curiosity', 'Merak', 'B1', NULL),
(47, 1, NULL, 'Reluctant', 'İsteksiz', 'C1', NULL),
(48, 1, NULL, 'Coincidence', 'Tesadüf', 'B2', NULL),
(49, 1, NULL, 'Obsolete', 'Modası geçmiş / Eskimiş', 'C1', NULL),
(50, 1, NULL, 'Vulnerable', 'Savunmasız / Hassas', 'C2', NULL),
(51, 1, NULL, 'Identify', 'Tanımlamak / Kimliğini saptamak', 'A2', NULL),
(52, 1, NULL, 'Solution', 'Çözüm', 'A2', NULL),
(53, 1, NULL, 'Generous', 'Cömert', 'B1', NULL),
(54, 1, NULL, 'Artificial', 'Yapay', 'B2', NULL),
(55, 2, NULL, 'Bankruptcy', 'İflas', 'C1', NULL),
(56, 2, NULL, 'Merger', 'Birleşme (Şirket)', 'C1', NULL),
(57, 2, NULL, 'Deadline', 'Son teslim tarihi', 'B1', NULL),
(58, 2, NULL, 'Asset', 'Varlık / Mal varlığı', 'B2', NULL),
(59, 2, NULL, 'Liability', 'Yükümlülük / Borç', 'C1', NULL),
(60, 2, NULL, 'Entrepreneur', 'Girişimci', 'B2', NULL),
(61, 2, NULL, 'Wholesale', 'Toptan Satış', 'B2', NULL),
(62, 2, NULL, 'Warranty', 'Garanti', 'B1', NULL),
(63, 2, NULL, 'Invoice', 'Fatura', 'A2', NULL),
(64, 2, NULL, 'Stakeholder', 'Paydaş', 'C1', NULL),
(65, 3, NULL, 'Symptom', 'Belirti / Semptom', 'B1', NULL),
(66, 3, NULL, 'Prescription', 'Reçete', 'B1', NULL),
(67, 3, NULL, 'Fatigue', 'Yorgunluk', 'C1', NULL),
(68, 3, NULL, 'Vaccine', 'Aşı', 'B1', NULL),
(69, 3, NULL, 'Contagious', 'Bulaşıcı', 'C1', NULL),
(70, 3, NULL, 'Dehydration', 'Su kaybı', 'B2', NULL),
(71, 3, NULL, 'Nutritious', 'Besleyici', 'B2', NULL),
(72, 3, NULL, 'Obesity', 'Obezite', 'B1', NULL),
(73, 3, NULL, 'Immune', 'Bağışık', 'B2', NULL),
(74, 3, NULL, 'Fracture', 'Kırık (Kemik)', 'C1', NULL);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `words`
--
ALTER TABLE `words`
  ADD PRIMARY KEY (`word_id`),
  ADD KEY `category_id` (`category_id`),
  ADD KEY `root_word_id` (`root_word_id`),
  ADD KEY `created_by` (`created_by`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `words`
--
ALTER TABLE `words`
  MODIFY `word_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=75;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `words`
--
ALTER TABLE `words`
  ADD CONSTRAINT `words_ibfk_1` FOREIGN KEY (`category_id`) REFERENCES `categories` (`category_id`) ON DELETE SET NULL,
  ADD CONSTRAINT `words_ibfk_2` FOREIGN KEY (`root_word_id`) REFERENCES `words` (`word_id`) ON DELETE SET NULL,
  ADD CONSTRAINT `words_ibfk_3` FOREIGN KEY (`created_by`) REFERENCES `users` (`user_id`) ON DELETE SET NULL;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
